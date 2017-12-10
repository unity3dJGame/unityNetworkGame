using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame
{
	using JGame.StreamObject;

	namespace Network
	{
		using JGame.Network.Setting;

		public class JNetworkDataOperator
		{
			private static object _sendLocker = new object();
			private static Semaphore _sendSemaphore = new Semaphore(0,1);
			private static object _receiveLocker = new object ();
			private static Semaphore _receivedSemaphore = new Semaphore (0, 1);

			public static void SendData (JNetworkPacketType packetType, byte [] data)
			{
				JNetworkData newData = new JNetworkData();
				newData.Data = data;
				newData.RemoteEndPoint = new  IPEndPoint(IPAddress.Parse(JNetworkServerInfo.ServerIP), JNetworkServerInfo.ServerPort);
				lock (_sendLocker) {
					JNetworkInteractiveData.SendData.Data.Enqueue (newData);
					_sendSemaphore.Release ();
				}
			}

			public static List<JNetworkData> TakeSendData()
			{
				_sendSemaphore.WaitOne ();
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

			public static void ReceivedData(int len, byte[] data, EndPoint remoteEndPoint)
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
				_receivedSemaphore.WaitOne ();
				List<JNetworkData> listData = new List<JNetworkData> ();

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
			public static JNetworkPacketType GetNetworkPacketType(JNetworkData data)
			{
				if (null == data)
					return JNetworkPacketType.npt_unknown;

				try
				{
					JInputStream inputStream = new JInputStream (data.Data);
					return (JNetworkPacketType)inputStream.Reader.ReadInt16 ();				
				}
				catch (Exception e) {
					JLog.Error ("JNetworkHelper.JNetworkPacketType error message :" + e.Message);
				}

				return JNetworkPacketType.npt_unknown;
			}

			public static bool IsValidPacketType(JNetworkPacketType type)
			{
				if (type > JNetworkPacketType.npt_min && type < JNetworkPacketType.npt_max) {
					return true;
				} else {
					return false;
				}
			}
		}//class JNetworkHelper
	}//namespace Network
}//namespace JGame

