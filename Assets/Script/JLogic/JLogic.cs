using System;
using System.Collections;
using System.Collections.Generic;

namespace JGame
{
	using JGame.Network;
	using JGame.Processer;

	namespace Logic
	{
		public static class JLogic
		{

			public static void Logic()
			{
				//根据状态调用processer处理

				List<JNetworkData> receivedData = JNetworkDataOperator.TakeReceivedData ();
				if (receivedData.Count > 0)
					return;

				foreach (JNetworkData data in receivedData) {
					ProcessNetworkData (data);
				}

			}

			public static void ProcessNetworkData(JNetworkData data)
			{
				JNetworkPacketType packetType = JNetworkHelper.GetNetworkPacketType (data);
				if (!JNetworkHelper.IsValidPacketType (packetType))
					return;

				IProcesser processor = JLogicHelper.getProcessor (packetType);
				if (null == processor)
					return;

				processor.run (JLogicUserData.Data);
			}
		}	
	}


}