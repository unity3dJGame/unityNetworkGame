using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame
{
	using JGame.StreamObject;
	using JGame.Data;

	namespace Network
	{
		using JGame.Network.Setting;

		public class JNetworkDataOperator
		{
			private static object _sendLocker = new object();
			private static Semaphore _sendSemaphore = new Semaphore(0,1);
			private static object _receiveLocker = new object ();
			private static Semaphore _receivedSemaphore = new Semaphore (0, 1);

			internal static void SendData (byte [] data)
			{
				JNetworkData newData = new JNetworkData();
				newData.Data = data;
				newData.RemoteEndPoint = new  IPEndPoint(IPAddress.Parse(JNetworkServerInfo.ServerIP), JNetworkServerInfo.ServerPort);
				lock (_sendLocker) {
					JNetworkInteractiveData.SendData.Data.Enqueue (newData);
					_sendSemaphore.Release ();
				}
			}
			public static void SendData(JPacketType packetType,  List<IStreamObj> objects)
			{
				JOutputStream jstream = new JOutputStream ();
				jstream.Writer.Write ((ushort)packetType);
				foreach(var obj in objects)
					JBinaryReaderWriter.Write (ref jstream, obj);

				SendData (jstream.ToArray ());
			}
			public static void SendData(JPacketType packetType,  IStreamObj streamObject)
			{
				JOutputStream jstream = new JOutputStream ();
				jstream.Writer.Write ((ushort)packetType);
				JBinaryReaderWriter.Write (ref jstream, streamObject);

				SendData (jstream.ToArray ());
			}

			/// <summary>
			/// Takes the send data.
			/// </summary>
			/// <returns>The send data. wait failed will return null</returns>
			/// <param name="millisecondsTimeout">Milliseconds timeout. <0 wait always</param>
			public static List<JNetworkData> TakeSendData(int millisecondsTimeout)
			{
				if (millisecondsTimeout < -1)
					_sendSemaphore.WaitOne ();
				else {
					if (!_sendSemaphore.WaitOne (millisecondsTimeout))
						return null;
				}
				
				List<JNetworkData> listData = new List<JNetworkData> ();

				try{
					lock (_sendLocker) {
						while (JNetworkInteractiveData.SendData.Data.Count > 0)
							listData.Add (JNetworkInteractiveData.SendData.Data.Dequeue ());
					}
				}
				catch (Exception e) {
					JLog.Error ("TakeSendData:" + e.Message, JGame.Log.JLogCategory.Network);
				}

				return listData;
			}
				
			public static void ReceiveData(int len, byte[] data, EndPoint remoteEndPoint)
			{
				JNetworkData networkData = new JNetworkData();
				networkData.Len = len;
				networkData.Data = data;
				networkData.RemoteEndPoint = remoteEndPoint;

				try
				{
					lock (_receiveLocker)
					{
						JNetworkInteractiveData.ReceivedData.Data.Enqueue(networkData);
						_receivedSemaphore.Release();
					}
				}
				catch (Exception e) {
					JLog.Error ("ReceiveData:" + e.Message, JGame.Log.JLogCategory.Network);
				}
			}
			public static List<JNetworkData> TakeReceivedData()
			{
				List<JNetworkData> listData = new List<JNetworkData> ();

				if (!_receivedSemaphore.WaitOne (1)) {
					return listData;
				}

				try{
					lock (_receiveLocker) {
						while (JNetworkInteractiveData.ReceivedData.Data.Count > 0)
							listData.Add (JNetworkInteractiveData.ReceivedData.Data.Dequeue ());
					}
				}
				catch (Exception e) {
					JLog.Error ("TakeSendData:" + e.Message, JGame.Log.JLogCategory.Network);
				}

				return listData;	
			}
		}//class JNetworkDataOperator

		public static class JNetworkHelper
		{
			public static JPacketType GetNetworkPacketType(JNetworkData data)
			{
				if (null == data)
					return JPacketType.npt_unknown;

				try
				{
					JInputStream inputStream = new JInputStream (data.Data);
					return (JPacketType)inputStream.Reader.ReadInt16 ();		
				}
				catch (Exception e) {
					JLog.Error ("JNetworkHelper.JPacketType error message :" + e.Message);
				}

				return JPacketType.npt_unknown;
			}

			public static bool IsValidPacketType(JPacketType type)
			{
				if (type > JPacketType.npt_min && type < JPacketType.npt_max) {
					return true;
				} else {
					return false;
				}
			}
		}//class JNetworkHelper
	}//namespace Network
}//namespace JGame

