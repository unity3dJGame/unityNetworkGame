using System;
using System.Collections;
using System.Collections.Generic;

namespace JGame
{
	using JGame.Network;
	using JGame.Processer;
	using JGame.StreamObject;
	using JGame.Data;
	using JGame.LocalData;

	namespace Logic
	{
		public static class JLogic
		{

			public static void Logic()
			{
				//根据状态调用processer处理
				//local
				List<JPacketType> addedLocalData = JLocalDataHelper.takeData ();
				if (addedLocalData.Count > 0)
				{
					JLog.Info ("JLogic.Logic find local data, count : " + addedLocalData.Count.ToString (), JGame.Log.JLogCategory.Network);
					foreach (JPacketType data in addedLocalData) 
					{
						ProcessLocalData (data);
					}
				}
				//network
				List<JNetworkData> receivedData = JNetworkDataOperator.TakeReceivedData ();
				if (receivedData.Count > 0) 
				{
					JLog.Info ("JLogic.Logic find network data, count : " + receivedData.Count.ToString (), JGame.Log.JLogCategory.Network);
					DeSerialize (receivedData);
					foreach (JNetworkData data in receivedData) 
					{
						ProcessNetworkData (data);
					}
				}

			}

			public static void ProcessNetworkData(JNetworkData data)
			{
				JPacketType packetType = JNetworkHelper.GetNetworkPacketType (data);
				if (!JNetworkHelper.IsValidPacketType (packetType))
					return;

				IProcesser processor = JLogicHelper.getProcessor (packetType);
				if (null == processor)
					return;

				processor.run (JLogicUserData.Data);
			}

			public static void ProcessLocalData(JPacketType data)
			{
				IProcesser processor = JLogicHelper.getProcessor (data);
				if (null == processor)
					return;

				processor.run (JLogicUserData.Data);
			}

			public static void DeSerialize(List<JNetworkData> dataList)
			{
				foreach (JNetworkData data in dataList) 
				{
					JInputStream stream = new JInputStream (data.Data);
					JBinaryReaderWriter.Read<ushort> (stream);
					if (stream.Stream.CanRead)
					{
						IStreamObj obj = JBinaryReaderWriter.Read<IStreamObj> (stream);
						if (null == obj)
							continue;
						
						JLogicUserData.Data.setData (obj);
					}
				}
			}
		}	
	}


}