using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JGame
{
	using StreamObject;
	namespace Data
	{
		
		public interface IDataSet
		{
			/// <summary>
			/// Sets the data.
			/// </summary>
			/// <param name="obj">Object.</param>
			void setData (IStreamObj obj);

			/// <summary>
			/// Gets the data.
			/// </summary>
			IStreamObj  getData(JObjectType type) ;
		}

		public class UserData:IDataSet
		{
			private Dictionary<ushort, IStreamObj> _myData = new Dictionary<ushort, IStreamObj>();
			private ReaderWriterLock _lock = new ReaderWriterLock ();

			public void setData (IStreamObj obj)
			{
				try
				{
					_lock.AcquireWriterLock (-1);
					if (!_myData.ContainsKey (obj.Type ()))
						_myData.Add (obj.Type (), obj);
					else
						_myData [obj.Type ()] = obj;
				}
				catch (Exception e) {
					JLog.Error (e.Message);
				}
				finally {
					_lock.ReleaseWriterLock ();
				}
			}

			public IStreamObj getData(JObjectType type)
			{
				IStreamObj obj = null;
				try
				{
					_lock.AcquireReaderLock(-1);
					if (_myData.ContainsKey ((ushort)type))
						obj = _myData [(ushort)type];
				}
				catch (Exception e) {
					JLog.Error (e.Message);
				}
				finally{
					_lock.ReleaseReaderLock ();
				}
				return obj;
			}
		}
	}
}