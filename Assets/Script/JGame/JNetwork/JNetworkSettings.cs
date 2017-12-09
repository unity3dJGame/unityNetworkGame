using System;
using System.Collections;
using System.Collections.Generic;


namespace JGame.Network.Setting
{
	public static class JNetworkInteractiveSettings
	{
		public static JNetworkDataQueue	ReceivedData {
			get;
			set;
		}
		public static JNetworkDataQueue SendData {
			get;
			set;
		}
	}

	public static class JNetworkServerInfo
	{
		public static string ServerIP {
			get;
			set;
		}
		public static int ServerPort {
			get;
			set;
		}
	}
}

