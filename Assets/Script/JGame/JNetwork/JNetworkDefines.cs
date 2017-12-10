using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace JGame.Network
{
	using JGame.StreamObject;

	public class JTcpDefines
	{
		public static int max_buffer_size = 1024;
		public static int min_buffer_size = 2;
		public static int max_tcp_connect = 1000;
	};
		
	public class JNetworkData
	{
		public int 			Len;
		public byte[] 		Data;
		//public EndPoint		LocalEndPoint;
		public EndPoint 	RemoteEndPoint;
	}

	public class JNetworkDataQueue
	{
		public Queue<JNetworkData> _dataQueue = new Queue<JNetworkData>();

		public Queue<JNetworkData> Data {
			get { return _dataQueue; }
		}

	}

}
