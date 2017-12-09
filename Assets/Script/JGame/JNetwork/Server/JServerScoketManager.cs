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

	public class JServerSocketManager
	{
		private static JServerSocketManager _manager = null;
		private static Socket 				_serverSocket = null;
		private static List<Socket>			_connectedClientSockets = null;
		private static Thread				_serverReceiveThread = null;
		private static Thread				_serverAcceptThread = null;
		private static bool					_initialzed = false;
		private static Semaphore			_semaphore = null;
		private static object				_socketLocker = null;
		private static object				_dataLocker = null;
		private static bool					_forceEnd = false; 

		private JServerSocketManager ()
		{
			_connectedClientSockets = new List<Socket> ();
		}

		public JServerSocketManager SingleInstance()
		{
			if (null == _manager) {
				JServerSocketManager manager = new JServerSocketManager ();
				System.Threading.Interlocked.CompareExchange<JServerSocketManager> (ref _manager, manager, null);
			}
			return _manager;
		}

		public void Shutdown()
		{
			_forceEnd = true;
		}

		public void Initialize(string serverIP, int serverPort)
		{
			if (_initialzed) {
				JLog.Error ("JServerSocketManager initialized aready !");
				return;
			}

			JLog.Info ("JServerSocketManager begin to initialize :");

			_socketLocker = new object ();
			_dataLocker = new object ();
			_semaphore = new Semaphore (0, 1);
			JNetworkInteractiveSettings.ReceivedData = new JNetworkDataQueue ();
			JNetworkInteractiveSettings.SendData = new JNetworkDataQueue ();
			IPAddress ip_server = IPAddress.Parse (serverIP); 
			IPEndPoint server_edp = new IPEndPoint (ip_server, serverPort);

			_serverSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				_serverSocket.Bind(server_edp);
				JLog.Info("JServerSocketManager server socket bind to server endpoint finished");

				_serverSocket.Listen(JTcpDefines.max_tcp_connect);
				JLog.Info("JServerSocketManager server socket begin listen");

				_serverReceiveThread = new Thread(ReceiveLoop) { IsBackground = true };
				_serverReceiveThread.Start();

				_serverAcceptThread = new Thread(AcceptLoop) { IsBackground = true };
				_serverAcceptThread.Start();
			}
			catch (Exception e) {
				JLog.Error ("JServerSocketManager initialize faield  error message: " + e.Message);
				return;
			}
		}

		private void AcceptLoop()
		{
			JLog.Info("JServerSocketManager server accept loop started", JGame.Log.JLogCategory.Network);

			while (true)
			{
				if (_forceEnd)
					break;
				
				try
				{
					Socket currentConnectedSocket = _serverSocket.Accept();
					if (null != currentConnectedSocket)
					{
						lock (_socketLocker)
						{
							if (!_connectedClientSockets.Contains(currentConnectedSocket))
							{
								_connectedClientSockets.Add(currentConnectedSocket);

								JLog.Info("client connected :"+currentConnectedSocket.RemoteEndPoint.ToString());
								_semaphore.Release();
							}
						}
					}
				}
				catch (Exception e)
				{
					JLog.Error ("JServerSocketManager accept loop error message:" + e.Message, JGame.Log.JLogCategory.Network);
				}
			}

			JLog.Info("JServerSocketManager server accept loop end.", JGame.Log.JLogCategory.Network);
		}

		private void ReceiveLoop()
		{
			JLog.Info ("JServerSocketManager server receive loop started", JGame.Log.JLogCategory.Network);

			List<Socket> clientScokets = new List<Socket>();
			while (true)
			{
				if (_forceEnd)
					break;
				
				if (_semaphore.WaitOne())
				{
					lock (_socketLocker)
					{
						foreach (Socket socket in _connectedClientSockets) 
						{
							clientScokets.Add (socket);
						}						
					}

					JLog.Debug ("connected client sockets : " + clientScokets.Count.ToString (), JGame.Log.JLogCategory.Network);
				}

				if (clientScokets.Count == 0 ) {
					_semaphore.WaitOne ();
					_semaphore.Release();
					continue;
				} 
					
				Socket.Select (clientScokets, null, null, 500);
				List<Socket> disconnectedSockets = new List<Socket>();
				foreach (Socket socket in clientScokets) 
				{
					//receive form client socket
					try
					{
						byte[] recBuffer = new byte[JTcpDefines.max_buffer_size];
						int recLen = socket.Receive (recBuffer);
						if (recLen > 0) {
							//save the received data
							JNetwrokData data = new JNetwrokData();
							//data.Len = recLen;
							data.Data = recBuffer;
							//data.LocalEndPoint = socket.LocalEndPoint;
							data.RemoteEndPoint = socket.RemoteEndPoint;
							lock (_dataLocker)
							{
								ReceiveData(data);
							}

							//add the selected socket to select sockets list
							clientScokets.Add(socket);
						}
						else 
						{
							//client disconnect
							socket.Close();
							//record disconnected socket from list
							disconnectedSockets.Add(socket);
	
							JLog.Info("client socket disconnected : " + socket.RemoteEndPoint.ToString());
						}
					}
					catch (Exception e) {
						JLog.Error ("JServerSocketManager ReceiveLoop exception error message:" + e.Message, JGame.Log.JLogCategory.Network);
					}
				}

				//remove disconnected socket form list
				if (disconnectedSockets.Count > 0) {
					lock (_socketLocker) {
						foreach ( Socket socket in disconnectedSockets)
						{
							_connectedClientSockets.Remove (socket);
							clientScokets.Remove (socket);
						}
					}
				}

			}

			JLog.Info ("JServerSocketManager server receive loop end.", JGame.Log.JLogCategory.Network);
		}

		private void ReceiveData(JNetwrokData data)
		{
			if (null == data)
				return;
			
			JNetworkInteractiveSettings.ReceivedData.Data.Enqueue(data);
		}		 
	}
}

