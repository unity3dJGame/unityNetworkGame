using System.Collections;
using System.Collections.Generic;

namespace JGame
{
	using JGame.Log;
	using JGame.Log.LogSettings;

	public class JLog
	{
		#region public part
		public static void Initialize (JLogMessagesQueue messages = null, List<JLogSettings> settings = null)
		{
			//
			JLogSpecifiedMessagesQueue.MessagesQueue = messages;
		}

		public static void Debug(string log, JLogCategory logCat= JLogCategory.Common)
		{
			WriteLog (log, JLogType.Debug, logCat);
		}
		public static void Info(string log, JLogCategory logCat= JLogCategory.Common)
		{
			WriteLog (log, JLogType.Info, logCat);
		}
		public static void Warn(string log, JLogCategory logCat= JLogCategory.Common)
		{
			WriteLog (log, JLogType.Warn, logCat);
		}
		public static void Error(string log, JLogCategory logCat= JLogCategory.Common)
		{
			WriteLog (log, JLogType.Error, logCat);
		}
		public static void Fatal(string log, JLogCategory logCat= JLogCategory.Common)
		{
			WriteLog (log, JLogType.Fatal, logCat);
		}
		#endregion

		#region private part
		private static void WriteLog(string log, JLogType logType, JLogCategory logCat= JLogCategory.Common)
		{
			
		}
		#endregion

	}


	public class JLogWriter
	{
	}

}
