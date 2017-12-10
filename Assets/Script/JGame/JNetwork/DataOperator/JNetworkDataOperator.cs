using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame.Network
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
			JNetwrokData newData = new JNetwrokData();
			newData.Data = data;
			newData.RemoteEndPoint = new  IPEndPoint(IPAddress.Parse(JNetworkServerInfo.ServerIP), JNetworkServerInfo.ServerPort);
			lock (_sendLocker) {
				JNetworkInteractiveData.SendData.Data.Enqueue (newData);
				_sendSemaphore.Release ();
			}
		}

		public static List<JNetwrokData> TakeSendData()
		{
			_sendSemaphore.WaitOne ();
			List<JNetwrokData> listData = new List<JNetwrokData> ();

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
			JNetwrokData networkData = new JNetwrokData();
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
		public List<JNetwrokData> TakeReceivedData(JNetworkPacketType type)
		{
			_receivedSemaphore.WaitOne ();
			List<JNetwrokData> listData = new List<JNetwrokData> ();

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
	}
}

