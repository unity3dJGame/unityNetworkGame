using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace JGame.Log
{
	public enum JLogType
	{
		[Description("调试")]
		Debug = 0,
		[Description("普通信息")]
		Info,
		[Description("警告")]
		Warn,
		[Description("错误")]
		Error,
		[Description("致命")]
		Fatal
	}

	public enum JLogCategory
	{
		[Description("常规")]
		Common = 0,
		[Description("网络")]
		Network,
		[Description("线程")]
		Thread,
	}

	public class JLogMessage
	{
		public static JLogType		LogType;
		public static JLogCategory	LogCategory;
		public static string 		LogMessage;
	}

	public class JLogMessagesQueue
	{
		public Queue<JLogMessage> LogMessages;
	}

}

