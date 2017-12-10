using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame
{
	using JGame.StreamObject;
	using JGame.Data;
	using JGame.Logic;

	namespace LocalData
	{
		public class JLocalDataHelper
		{
			private static object _locker = new object();
			private static Semaphore _semaphore = new Semaphore(0,1);


			public static void addData( JPacketType type, IStreamObj obj)
			{
				try
				{
					JLogicUserData.Data.setData (obj);
					lock (_locker)
					{
						JLocalDataProcessors.Data.Enqueue (type);
					}
					_semaphore.Release();
				}
				catch(Exception e) {
					JLog.Error ("JLocalDataHelper: addData " + e.Message);
				}
			}

			public static List<JPacketType> takeData()
			{
				List<JPacketType> listData = new List<JPacketType> ();
				if(!_semaphore.WaitOne (1))
					return listData;

				try{
					lock (_locker) {
						while (JLocalDataProcessors.Data.Count > 0)
							listData.Add (JLocalDataProcessors.Data.Dequeue());
					}
				}
				catch (Exception e) {
					JLog.Error ("JLocalDataHelper:takeData :" + e.Message, JGame.Log.JLogCategory.Network);
				}

				return listData;
			}
		}
			
	}//namespace Network
}//namespace JGame

