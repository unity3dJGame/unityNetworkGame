using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace JGame.Log
{
	public enum JLogType
	{
		[Description("[Debug]")]
		Debug = 0,
		[Description("[Info ]")]
		Info,
		[Description("[warn ]")]
		Warn,
		[Description("[Error]")]
		Error,
		[Description("[Fatal]")]
		Fatal
	}

	public enum JLogCategory
	{
		[Description("Common")]
		Common = 0,
		[Description("Network")]
		Network,
		[Description("Thread")]
		Thread,
	}

	public class JLogMessage
	{
		public JLogType		LogType { set; get; }
		public JLogCategory	LogCategory { set; get; }
		public string 		LogMessage { set; get; }
	}

	public class JLogMessagesQueue
	{
		public Queue<JLogMessage> LogMessages;
	}

}

