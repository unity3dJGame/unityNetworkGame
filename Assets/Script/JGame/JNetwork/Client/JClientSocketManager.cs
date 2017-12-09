﻿using System;
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

		private JClientSocketManager ()
		{
		}

		public void ShutDown()
		{
			JClientDataSenderThread.ShutDown ();
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
			JClientSocket.socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			JNetworkInteractiveData.ReceivedData = new JNetworkDataQueue ();
			JNetworkInteractiveData.SendData = new JNetworkDataQueue ();

			try
			{
				JClientSocket.socket.Connect(serverEdp);
				JLog.Info("Connect to server success.");

				JClientDataSenderThread.Initialize();
			}
			catch (Exception e) {
				JLog.Error (e.Message, JGame.Log.JLogCategory.Network);
				return;
			}
		}

		public void SendData(JNetworkPacketType packetType, byte[] data)
		{
			JNetworkDataOperator.SendData (packetType, data);
		}
	}
}

