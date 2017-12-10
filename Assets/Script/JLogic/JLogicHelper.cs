using System;
using System.Threading;

namespace JGame
{
	using JGame.Network;
	using JGame.Processer;
	using JGame.Data;

	namespace Logic
	{
		public static class JLogicHelper
		{
			public static bool IsServer = false;
			private static ReaderWriterLockSlim _readerWriteLocker = new ReaderWriterLockSlim();

			public static bool registerProcessor(JPacketType packetType, IProcesser processor, bool isServerProcessor = false)
			{
				if (IsServer && !isServerProcessor || !IsServer && isServerProcessor)
					return true;
				
				bool bSuccess = false;
				try
				{
					_readerWriteLocker.EnterWriteLock ();

					if (!JLogicRegisteredProcessors.processors.PacketTypeToProcessor.ContainsKey(packetType))
					{
						JLogicRegisteredProcessors.processors.PacketTypeToProcessor.Add(packetType, processor);
						bSuccess = true;
					}

						
				}
				catch (Exception e) {
					JLog.Error ("JLogicHelper.RegisterProcessor error message:" + e.Message);
				}
				finally {
					_readerWriteLocker.ExitWriteLock ();
				}

				return bSuccess;
			}

			public static IProcesser getProcessor(JPacketType packetType)
			{
				IProcesser processor = null;
				try
				{
					_readerWriteLocker.EnterReadLock ();

					if (JLogicRegisteredProcessors.processors.PacketTypeToProcessor.ContainsKey(packetType))
					{
						processor = (IProcesser)JLogicRegisteredProcessors.processors.PacketTypeToProcessor[packetType];
					}
				}
				catch (Exception e) {
					JLog.Error ("JLogicHelper.RegisterProcessor error message:" + e.Message);
				}
				finally {
					_readerWriteLocker.ExitReadLock ();
				}

				if (null == processor)
					JLog.Error("JLogicHelper.RegisterProcessor: Unregistered packet type:" + packetType.GetDescription());

				return processor;
			}
		}
	}
}

