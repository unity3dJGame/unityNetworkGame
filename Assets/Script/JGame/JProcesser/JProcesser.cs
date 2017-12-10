using System;
using System.Net;
using System.Net.Sockets;

namespace JGame
{
	using StreamObject;
	using Network;
	using Data;

	namespace Processer
	{

		public interface IProcesser 
		{
			void run (IDataSet dataSet);
		}
	}
}

