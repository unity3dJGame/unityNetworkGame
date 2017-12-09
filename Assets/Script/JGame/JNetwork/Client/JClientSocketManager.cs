using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame.Network
{
	using JGame.StreamObject;
	using JGame.Network.Setting;
	public class JClientSocketManager
	{
		private static JClientSocketManager	_manager = null;
		private static Socket				_clientSockets = null;
		private static List<IPEndPoint>		_serverIPEndPoints = null;


		private JClientSocketManager ()
		{
			_serverIPEndPoints = new List<IPEndPoint> ();
		}

		public JClientSocketManager SingleInstance()
		{
			if (null == _manager) {
				JClientSocketManager manager = new JClientSocketManager ();
				System.Threading.Interlocked.CompareExchange<JClientSocketManager> (ref _manager, manager, null);
			}
			return _manager;
		}

		public void Initialize(string serverIP, int serverPort)
		{
			JNetworkServerInfo.ServerIP = serverIP;
			JNetworkServerInfo.ServerPort = serverPort;
			IPAddress serverAdress = IPAddress.Parse (serverIP);
			IPEndPoint serverEdp = new IPEndPoint (serverAdress, serverPort);
			_clientSockets = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				_clientSockets.Connect(serverEdp);
				JLog.Info("Connect to server success.");
			}
			catch (Exception e) {
				JLog.Error (e.Message, JGame.Log.JLogCategory.Network);
				return;
			}
		}

		public void SendData(JNetworkPacketType packetType, byte[] data)
		{
			JNetworkHelper.SendData (packetType, data);
		}
	}
}

