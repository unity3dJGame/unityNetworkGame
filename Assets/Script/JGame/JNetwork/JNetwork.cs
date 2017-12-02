using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame.Network
{
	public class JTcpUtil
	{
		public const int max_buffer_size = 1024;
		public const int min_buffer_size = 2;

	}

	public enum JNetworkPacketType
	{
		npt_min = 0,

		npt_signin,
		npt_signin_ret,

		npt_max
	}

}
