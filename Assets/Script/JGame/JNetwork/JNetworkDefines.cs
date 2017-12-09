using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace JGame.Network
{
	using JGame.StreamObject;

	public class JTcpDefines
	{
		public static int max_buffer_size = 1024;
		public static int min_buffer_size = 2;
		public static int max_tcp_connect = 1000;
	};

	public enum JNetworkPacketType
	{
		npt_min = 0,

		npt_signin_hit,
		npt_signin_ret,

		npt_max
	};

	public class JNetwrokData
	{
		public int 			Len;
		public byte[] 		Data;
		//public EndPoint		LocalEndPoint;
		public EndPoint 	RemoteEndPoint;
	}

	public class JNetworkDataQueue
	{
		public Queue<JNetwrokData> _dataQueue = new Queue<JNetwrokData>();

		public Queue<JNetwrokData> Data {
			get { return _dataQueue; }
		}

	}

}
