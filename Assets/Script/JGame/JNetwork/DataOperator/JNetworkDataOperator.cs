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
		public static Semaphore _sendSemaphore = new Semaphore(0,1);

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
				JLog.Error ("TakeSendData:" + e.Message);
			}

			return listData;
		}

		/*public JNetwrokData ReceiveData(JNetworkPacketType type)
		{
			
		}*/
	}
}

