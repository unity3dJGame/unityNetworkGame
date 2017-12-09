using System;
using System.Net;

namespace JGame.Network
{
	using JGame.Network.Setting;
	public class JNetworkHelper
	{
		private static object _sendLocker = new object();

		public static void SendData (JNetworkPacketType packetType, byte [] data)
		{
			JNetwrokData newData = new JNetwrokData();
			newData.Data = data;
			newData.RemoteEndPoint = new  IPEndPoint(IPAddress.Parse(JNetworkServerInfo.ServerIP), JNetworkServerInfo.ServerPort);
			lock (_sendLocker) {
				JNetworkInteractiveSettings.SendData.Data.Enqueue (newData);
			}
		}

		/*public JNetwrokData ReceiveData(JNetworkPacketType type)
		{
			
		}*/
	}
}

