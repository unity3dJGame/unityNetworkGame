using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace JGame.Network.Setting
{
	public static class JNetworkInteractiveData
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

	public static class JClientSocket
	{
		public static Socket  socket 
		{
			get;
			set;
		}
	}

	public static class JServerSocket
	{
		public static Socket socket
		{
			get;
			set;
		}
	}

	public static class JConnectedClientSocket
	{
		public static List<Socket> sockets
		{
			get;
			set;
		}
	}


}

