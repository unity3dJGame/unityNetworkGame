using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame.Log
{
	using JGame.Log.LogSettings;

	public class JLogThread
	{
		private static Dictionary<string, StreamWriter> _writers;
		private static object _message_lock;
		private static object _setting_lock;
		private static Semaphore _semaphore;
		private static bool  _inited = false;

		public static void Initialize()
		{
			_message_lock = new object ();
			_setting_lock = new object ();
			_semaphore = new Semaphore (0, int.MaxValue, "JLogSemaphore");
			_writers = new Dictionary<string, StreamWriter> ();
			Thread thread = new Thread (run) { IsBackground = true };
			thread.Start ();
			_inited = true;
		}

		public static void AddMessage(JLogMessage message)
		{
			try
			{
				lock (_message_lock) 
				{
					if (null != JLogSpecifiedMessagesQueue.MessagesQueue.LogMessages) 
					{
						JLogSpecifiedMessagesQueue.MessagesQueue.LogMessages.Enqueue(message);
						_semaphore.Release();
					}
				}
			}
			catch (Exception e)
			{
				//no need to deal
			}
		}

		public static JLogMessage TakeMessage()
		{	
			try
			{
				lock (_message_lock)
				{
					if (JLogSpecifiedMessagesQueue.MessagesQueue.LogMessages.Count > 0)
					{
						return JLogSpecifiedMessagesQueue.MessagesQueue.LogMessages.Dequeue();
					}
				}
			}
			catch(Exception e) {
			}

			return null;
		}

		private static void run()
		{
			while (true) 
			{
				JLogMessage message = TakeMessage();
				if (null != message) 
				{
					WriteMessageToFile (message);
				}
				else if (!WaitLogMessage())
				{
					break;
				}
			}
		}


		private static bool WaitLogMessage()
		{
			WaitHandle.WaitAny (new WaitHandle[]{ _semaphore }, -1, false);
			return false;
		}

		private static void WriteMessageToFile(JLogMessage writeMsg)
		{
			lock (_setting_lock)
			{
				if (!_inited)
					return;
				try
				{

					foreach( JLogSettings setting in JLogSpecifiedSettingsSet.JLogSettingsList)
					{
						string strFileName = string.Format("{0}{1}{2}{3}",
							setting.LogFileDir, setting.LogFileNamePrefix, setting.LogFileNameStem, setting.LogFileNameSuffix);

						if (!_writers.ContainsKey(strFileName))
						{
							StreamWriter writer = new StreamWriter(strFileName, true, System.Text.Encoding.UTF8);
						}
						else{
							string msgText = string.Format("{0}{1}{2}", 
								writeMsg.LogType.GetDescription(), writeMsg.LogCategory.GetDescription(), writeMsg.LogMessage);
							_writers[strFileName].WriteLine(msgText);
						}
					}
				}
				catch(Exception e)
				{
				}
			}
		}
	}
}

