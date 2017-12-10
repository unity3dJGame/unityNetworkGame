using System;
using System.Threading;
using System.Collections.Generic;

namespace JGame.Network
{
	using JGame.Network.Setting;

	public static class JClientDataReceiverThread
	{
		private static bool _requireEnd = false;

		public static void ShutDown()
		{
			_requireEnd = true;
		}

		public static void Initialize()
		{
			_requireEnd = false;
			Thread thread = new Thread (MainLoop);
			thread.Start ();
		}

		public static void MainLoop()
		{
			if (null == JClientSocket.socket) {
				JLog.Error ("JClientDataReceiverThread.MainLoop JClientSocket.socket is null, please make sure JClientSocketManager is initialized. ", JGame.Log.JLogCategory.Network);
				return;
			}
			while (true) 
			{
				if (_requireEnd)
					break;

				if (JClientSocket.socket.Available <= 0) {
					Thread.Sleep (100);
					continue;
				}

				byte[] buffer = new byte[JTcpDefines.max_buffer_size];
				int recLen = JClientSocket.socket.Receive (buffer);
				if (recLen > 0) {
					JNetworkDataOperator.ReceivedData (recLen, buffer, JClientSocket.socket.RemoteEndPoint);
				} else {
					//说明socket已经断开连接
				}
			}
		}

	}
}

