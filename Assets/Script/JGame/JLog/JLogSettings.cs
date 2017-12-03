using System;
using System.Collections;
using System.Collections.Generic;

namespace JGame.Log.LogSettings
{
	public class JLogSpecifiedSettingsSet
	{
		public static List<JLogSettings> JLogSettingsList {
			set;
			get;
		}
	}
	public class JLogSpecifiedMessagesQueue
	{
		public static JLogMessagesQueue MessagesQueue
		{
			set;
			get;
		}
	}

	public class JLogSettings
	{
		public JLogSpecifiedMessagesQueue SpecifiedMsgsQueu 
		{
			set;
			get;
		}
		public string LogFileDir 
		{
			set;
			get;
		}
		public Dictionary<JLogType, string> LogFileNamePrefix {
			set;
			get;
		}
		public Dictionary<JLogCategory, string> LogFileNameStem {
			set;
			get;
		}
		public string LogFileNameSuffix {
			set;
			get;
		}
	}

}

