using System;
using System.Threading;
using System.Collections.Generic;

namespace JGame.Network
{
	using JGame.Network.Setting;

	public static class JClientDataSenderThread
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
			if (null == JNetworkInteractiveData.SendData ||
			    null == JNetworkInteractiveData.SendData.Data) {
				JLog.Error ("JNetworkDataSenderThread: JNetworkInteractiveData checked empty, please check initialize", JGame.Log.JLogCategory.Network);
				return;
			}

			if (null == JClientSocket.socket) {
				JLog.Error ("JNetworkDataSenderThread: JClientSocket.socket is null, please check initialize", JGame.Log.JLogCategory.Network);
				return;
			}

			JLog.Info("JNetworkDataSenderThread: main loop started", JGame.Log.JLogCategory.Network);

			while (true) {
				if (_requireEnd)
					break;

				try
				{
					List<JNetwrokData> dataList = JNetworkDataOperator.TakeSendData();
					foreach (JNetwrokData data in dataList)
					{
						JClientSocket.socket.Send(data.Data);
						JLog.Debug("JNetworkDataSenderThread: send one network packet", JGame.Log.JLogCategory.Network);
					}
				}
				catch (Exception e) {
					JLog.Error (e.Message, JGame.Log.JLogCategory.Network);
				}

			}

			JLog.Info("JNetworkDataSenderThread main loop end.", JGame.Log.JLogCategory.Network);
		}
	}
}

