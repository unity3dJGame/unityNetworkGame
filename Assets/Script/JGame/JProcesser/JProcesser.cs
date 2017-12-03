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
			/*/// <summary>
			/// Set the specified obj.
			/// </summary>
			/// <param name="obj">Object.</param>
			bool set(IStreamObj obj);

			/// <summary>
			/// Get the specified obj.
			/// </summary>
			/// <param name="obj">Object.</param>
			bool get(ref IStreamObj obj);*/

			void run (IDataSet dataSet);
		}
	}
}

