using System;
using System.Collections;
using System.Collections.Generic;
using System.

namespace JGame.Log.LogSettings
{
	public class JLogSettings
	{
		public static JLogSpecifiedMessagesQueue SpecifiedMsgsQueu 
		{
			set;
			get;
		}
		public static string LogFileDir 
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

	public class JLogSpecifiedMessagesQueue
	{
		public JLogMessagesQueue MessagesQueue
		{
			set;
			get;
		}
	}
}

