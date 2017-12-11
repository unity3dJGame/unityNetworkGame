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
		private static Thread				_serverReceiveThread = null;
		private static Thread				_serverAcceptThread = null;
		private static bool					_initialized = false;
		private static Semaphore			_semaphore = null;
		private static object				_socketLocker = null;
		private static bool					_forceEnd = false; 

		private JServerSocketManager ()
		{
			JConnectedClientSocket.sockets = new List<Socket> ();
		}

		public static JServerSocketManager SingleInstance
		{
			get {
				if (null == _manager) {
					JServerSocketManager manager = new JServerSocketManager ();
					System.Threading.Interlocked.CompareExchange<JServerSocketManager> (ref _manager, manager, null);
				}
				return _manager;
			}
		}

		public void ShutDown()
		{
			_forceEnd = true;
			try
			{
				JServerSocket.socket.Close ();
			}
			catch (Exception e) {
				JLog.Error (e.Message, JGame.Log.JLogCategory.Network);
			}
		}

		public bool Initialized
		{
			get { return _initialized; }
		}

		public void Initialize(string serverIP, int serverPort)
		{
			if (_initialized) {
				JLog.Error ("JServerSocketManager initialized aready !", JGame.Log.JLogCategory.Network);
				return;
			}

			JLog.Info ("JServerSocketManager begin to initialize :", JGame.Log.JLogCategory.Network);

			_socketLocker = new object ();
			_semaphore = new Semaphore (0, 1);
			JNetworkInteractiveData.ReceivedData = new JNetworkDataQueue ();
			JNetworkInteractiveData.SendData = new JNetworkDataQueue ();
			IPAddress ip_server = IPAddress.Parse (serverIP); 
			IPEndPoint server_edp = new IPEndPoint (ip_server, serverPort);

			JServerSocket.socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				JServerSocket.socket.Bind(server_edp);
				JLog.Info("JServerSocketManager server socket bind to server endpoint finished.\nIP:"+serverIP+"\nPort:"+serverPort.ToString(),
					JGame.Log.JLogCategory.Network);

				JServerSocket.socket.Listen(JTcpDefines.max_tcp_connect);
				JLog.Info("JServerSocketManager server socket begin listen", JGame.Log.JLogCategory.Network);

				_serverReceiveThread = new Thread(ReceiveLoop) { IsBackground = true };
				_serverReceiveThread.Start();

				_serverAcceptThread = new Thread(AcceptLoop) { IsBackground = true };
				_serverAcceptThread.Start();
			}
			catch (Exception e) {
				JLog.Error ("JServerSocketManager initialize faield  error message: " + e.Message, JGame.Log.JLogCategory.Network);
				return;
			}
		}

		private void AcceptLoop()
		{
			JLog.Info("JServerSocketManager server accept loop started", JGame.Log.JLogCategory.Network);

			while (true)
			{
				//JLog.Debug("AcceptLoop loop one begin ...", JGame.Log.JLogCategory.Network);

 				if (_forceEnd)
					break;
				
				try
				{					
					Socket currentConnectedSocket = JServerSocket.socket.Accept();
					if (null != currentConnectedSocket)
					{
						lock (_socketLocker)
						{
							if (!JConnectedClientSocket.sockets.Contains(currentConnectedSocket))
							{
								JConnectedClientSocket.sockets.Add(currentConnectedSocket);

								JLog.Info("client connected :"+currentConnectedSocket.RemoteEndPoint.ToString(), JGame.Log.JLogCategory.Network);
								_semaphore.Release();
							}
						}
					}
				}
				catch (Exception e)
				{
					JLog.Error ("JServerSocketManager accept loop error message:" + e.Message, JGame.Log.JLogCategory.Network);
				}

				//JLog.Debug("AcceptLoop loop one end ...", JGame.Log.JLogCategory.Network);
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
						foreach (Socket socket in JConnectedClientSocket.sockets) 
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
					if (socket.Available <= 0)
						continue;
					
					//receive form client socket
					try
					{
						byte[] recBuffer = new byte[JTcpDefines.max_buffer_size];
						int recLen = socket.Receive (recBuffer);
						if (recLen > 0) {
							//save the received data
							JNetworkDataOperator.ReceiveData(recLen, recBuffer, socket.RemoteEndPoint);

							//add the selected socket to select sockets list
							clientScokets.Add(socket);
						}
						else 
						{
							//client disconnect
							socket.Close();
							//record disconnected socket from list
							disconnectedSockets.Add(socket);
	
							JLog.Info("client socket disconnected : " + socket.RemoteEndPoint.ToString(), JGame.Log.JLogCategory.Network);
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
							JConnectedClientSocket.sockets.Remove (socket);
							clientScokets.Remove (socket);
						}
					}
				}

			}

			JLog.Info ("JServerSocketManager server receive loop end.", JGame.Log.JLogCategory.Network);
		}
	}
}

