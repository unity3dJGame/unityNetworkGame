using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.ComponentModel;

namespace JGame.StreamObject
{
	/// <summary>
	/// object类型
	/// </summary>
	public enum JObjectType
	{
		[Description("unknown type")]
		hot_unknown = 0,
		[Description("start of object types")]
		hot_start = 1,

		[Description("sign in require")]
		sign_in,	//登录包
		[Description("sign in return")]
		sign_in_ret,	//登录包

		[Description("end of object types")]
		hot_end
	}

	/// <summary>
	/// interface of stream object.
	/// </summary>
	public interface IStreamObj
	{
		/// <summary>
		/// Headers the stream object type.
		/// </summary>
		/// <returns>The type.</returns>
		ushort Type ();

		/// <summary>
		/// Read the specified data.
		/// </summary>
		/// <param name="data">input data</param>
		void Read (ref JInputStream stream);

		/// <summary>
		/// Write the specified stream.
		/// </summary>
		/// <param name="stream">out put stream</param>
		void Write (ref JOutputStream stream);
	}

	public class JOutputStream
	{
		public MemoryStream Stream {
			set;
			get ;
		}

		public BinaryWriter Writer {
			set;
			get ;
		}

		public JOutputStream()
		{
			Stream = new MemoryStream ();
			Writer = new BinaryWriter (Stream);
		}
		public JOutputStream(ref MemoryStream stream)
		{
			if (null == stream)
				Stream = new MemoryStream ();

			Writer = new BinaryWriter (Stream);
		}

		public void Flush()
		{
			Writer.Flush ();
		}

		public byte[] ToArray()
		{
			return Stream.ToArray ();
		}
	}

	public class JInputStream
	{
		public MemoryStream Stream {
			set;
			get;
		}

		public BinaryReader Reader {
			set;
			get;
		}

		public JInputStream(byte[] data)
		{
			Stream = new MemoryStream(data);
			Reader = new BinaryReader (Stream);	
		}
		public JInputStream(ref MemoryStream stream)
		{
			Stream = stream;
			Reader = new BinaryReader (Stream);
		}
	}
}