using System;
using System.ComponentModel;

namespace JGame.Data
{
	public enum JPacketType
	{
		[Description("未知")]
		npt_unknown = 0,
		[Description("Packet type min define")]
		npt_min = 1,

		[Description("登录请求包")]
		npt_signin_req,
		[Description("登录返回包")]
		npt_signin_ret,

		[Description("Packet type max define")]
		npt_max
	};
}

