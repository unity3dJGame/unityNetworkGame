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
			if (null == messages) {
				messages = new JLogMessagesQueue ();
				messages.LogMessages = new Queue<JLogMessage> ();
			}

			if (null == settings) {

				settings = new List<JLogSettings> ();
				JLogSettings logst = new JLogSettings();
				logst.LogFileDir = string.Format("{0}/log/",System.Environment.CurrentDirectory);
				logst.LogFileNamePrefix = new Dictionary<JLogType, string> { };
				logst.LogFileNameStem = 
					new Dictionary<JLogCategory, string> {  {JLogCategory.Common, ""}, 
															{JLogCategory.Network, "Network"},
															{JLogCategory.Thread, "Thread"} };
				
				logst.LogFileNameSuffix = ".txt";
				settings.Add (logst);
			}
			JLogSpecifiedSettingsSet.JLogSettingsList = settings;
			JLogSpecifiedMessagesQueue.MessagesQueue = messages;

			JLogThread.Initialize ();
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
			JLogMessage msg = new JLogMessage { LogType = logType,LogCategory = logCat, LogMessage = log };
			JLogThread.AddMessage (msg);
		}
		#endregion

	}
}
