using System;

namespace JGame
{
	using Logic;
	using JGame.Data;

	namespace Processer
	{
		public static class ProcessorRegister
		{
			public static void RegisterServerProcessor()
			{

			}

			public static void RegisterClientProcessor()
			{
				//JLogicHelper.registerProcessor (JPacketType.npt_signin_req, new JProcesserSignInSet (), false);
				//JLogicHelper.registerProcessor (JPacketType.npt_signin_ret, new JProcesserSignInGet (), false);
			}
		}
	}
}

