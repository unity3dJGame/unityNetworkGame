using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JGame.StreamObject
{

	/// <summary>
	/// 支持基础值类型和JStreamObject的读写
	/// </summary>
	public class JBinaryReaderWriter  {
		/// <summary>
		/// Read the specified data,start at start_index.
		/// </summary>
		/// <param name="data">data to read</param>
		/// <param name="start_index">Start index of data to read</param>
		/// <returns> offset of data</returns>
		public static T Read<T> (JInputStream jstream)
		{
			if (typeof(T) == typeof(string)) {
				ushort len = (ushort)jstream.Reader.ReadInt16 ();
				return (T)Convert.ChangeType(Encoding.UTF8.GetString(jstream.Reader.ReadBytes (len)), typeof(T));
			}
			if (typeof(T).IsValueType) {
				if (typeof(T) == typeof(short)) {
					short result = (short)jstream.Reader.ReadInt16 ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(ushort)) {
					ushort result = (ushort)jstream.Reader.ReadUInt16 ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(int)) {
					int result = (int)jstream.Reader.ReadInt32 ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(uint)) {
					uint result = (uint)jstream.Reader.ReadUInt32 ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(long)) {
					long result = (long)jstream.Reader.ReadInt64 ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(ulong)) {
					ulong result = (ulong)jstream.Reader.ReadUInt64 ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(float)) {
					float result = (float)jstream.Reader.ReadSingle ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(double)) {
					double result = (double)jstream.Reader.ReadDouble ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(decimal)) {
					decimal result = (decimal)jstream.Reader.ReadDecimal ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(bool)) {
					bool result = (bool)jstream.Reader.ReadBoolean ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(byte)) {
					byte result = (byte)jstream.Reader.ReadByte ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(sbyte)) {
					sbyte result = (sbyte)jstream.Reader.ReadSByte ();
					return (T)Convert.ChangeType (result, typeof(T));
				} else if (typeof(T) == typeof(char)) {
					char result = (char)jstream.Reader.ReadChar ();
					return (T)Convert.ChangeType (result, typeof(T));
				} //else if (typeof(T).IsEnum) {}
			} else if (typeof(T).IsInterface || typeof(T).IsClass) {
				ushort objType = (ushort)jstream.Reader.ReadUInt16 ();
				IStreamObj obj = JGameUtil.InvokeStreamObject (objType);
				obj.Read (ref jstream);
				return (T)obj;
			}

			JLog.Error ("JBinaryReaderWriter.Read:not support type" + typeof(T).ToString ());
			return default(T);
		}


		public static void Write<T>(ref JOutputStream jstream, T inputObj)
		{
			if (inputObj.GetType () == typeof(byte[])) 
			{
				byte[] bytes = (byte[])Convert.ChangeType(inputObj, typeof(byte[]));
				jstream.Writer.Write ((ushort)bytes.Length);
				jstream.Writer.Write (bytes);
			}
			if (inputObj.GetType () == typeof(char[])) 
			{
				char[] chars = (char[])Convert.ChangeType(inputObj, typeof(char[]));
				byte[] bytes = Encoding.UTF8.GetBytes (chars, 0, chars.Length);
				jstream.Writer.Write ((ushort)bytes.Length);
				jstream.Writer.Write (bytes);
			}
			if (inputObj.GetType () == typeof(string)) {
				string str = inputObj as string;
				byte[] bytes = Encoding.UTF8.GetBytes (str);
				jstream.Writer.Write ((ushort)bytes.Length);
				jstream.Writer.Write (bytes);
			} else if (inputObj.GetType ().IsValueType) {
				if (inputObj.GetType () == typeof(decimal)) {
					decimal value = (decimal)Convert.ChangeType (inputObj, typeof(decimal));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(bool)) {
					bool value = (bool)Convert.ChangeType (inputObj, typeof(bool));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(char)) {
					char value = (char)Convert.ChangeType (inputObj, typeof(char));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(byte)) {
					byte value = (byte)Convert.ChangeType (inputObj, typeof(byte));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(sbyte)) {
					sbyte value = (sbyte)Convert.ChangeType (inputObj, typeof(sbyte));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(short)) {
					short value = (short)Convert.ChangeType (inputObj, typeof(short));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(ushort)) {
					ushort value = (ushort)Convert.ChangeType (inputObj, typeof(ushort));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(int)) {
					int value = (int)Convert.ChangeType (inputObj, typeof(int));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(uint)) {
					uint value = (uint)Convert.ChangeType (inputObj, typeof(uint));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(long)) {
					long value = (long)Convert.ChangeType (inputObj, typeof(long));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(ulong)) {
					ulong value = (ulong)Convert.ChangeType (inputObj, typeof(ulong));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(float)) {
					float value = (float)Convert.ChangeType (inputObj, typeof(float));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType () == typeof(double)) {
					double value = (double)Convert.ChangeType (inputObj, typeof(double));
					jstream.Writer.Write (value);
				} else if (inputObj.GetType ().IsEnum) {
					JLog.Error ("JBinaryReaderWriter.Write: not support enum type!");
				} else {
					JLog.Error ("JBinaryReaderWriter.Write: not support this value type:"  + inputObj.GetType().ToString ());
				}
			} else if (inputObj.GetType ().IsClass) {
				IStreamObj obj = inputObj as IStreamObj;
				if (null != obj) {
					obj.Write (ref jstream);
				} else
					JLog.Error ("JBinaryReaderWriter.Write: not support this class type:"  + inputObj.GetType().ToString ());
			} else {
				JLog.Error ("JBinaryReaderWriter.Write: unknown type:" + inputObj.GetType().ToString ());
			}
		}
	}
}