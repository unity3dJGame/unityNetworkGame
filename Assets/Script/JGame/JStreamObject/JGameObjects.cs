using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JGame.StreamObject
{
	
	/// <summary>
	/// 登录信息object
	/// </summary>
	public class JObj_SignIn : IStreamObj
	{
		public string _strAccount = "";
		public string _strCode = "";

		public ushort Type ()
		{
			return (ushort)JObjectType.sign_in;
		}
		public void Read (ref JInputStream stream)
		{
			_strAccount = JBinaryReaderWriter.Read<string> (stream);
			_strCode = JBinaryReaderWriter.Read<string> (stream);
		}
		public void Write (ref JOutputStream stream)
		{ 
			if (null == stream)
				stream = new JOutputStream ();
			JBinaryReaderWriter.Write(ref stream, Type());
			JBinaryReaderWriter.Write (ref stream, _strAccount);
			JBinaryReaderWriter.Write (ref stream, _strCode);
			stream.Flush ();
		}
	}

	public class JObj_SignRet: IStreamObj
	{
		public bool Result = false;

		public ushort Type ()
		{
			return (ushort)JObjectType.sign_in_ret;
		}
		public void Read (ref JInputStream stream)
		{
			Result = JBinaryReaderWriter.Read<bool> (stream);
		}
		public void Write (ref JOutputStream stream)
		{ 
			if (null == stream)
				stream = new JOutputStream ();
			JBinaryReaderWriter.Write(ref stream, Type());
			JBinaryReaderWriter.Write (ref stream, Result);
			stream.Flush ();
		}
	}
}
