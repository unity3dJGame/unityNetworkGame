using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;

namespace JGame
{
	using JGame.Log;

	public class JGameManager
	{
		private static JGameManager _singleInstance;
		private JGameManager(){}
		public static JGameManager SingleInstance
		{
			get
			{
				if (null == _singleInstance) {
					JGameManager manager = new JGameManager ();
					System.Threading.Interlocked.CompareExchange (ref _singleInstance, manager, null);
				}
				return _singleInstance;
			}
		}


		private Dictionary<ushort,  string>  _typeToTypeName
		{
			set;
			get;
		}
		private HashSet<string> _toLoadNamespaces
		{
			set;
			get;
		}

		public void initialize()
		{
			_typeToTypeName = new Dictionary<ushort, string> ();
			_toLoadNamespaces = new HashSet<string> ();
			_toLoadNamespaces.Add ("JGame.StreamObject");

			//initialize log system.
			JLog.Initialize ();

			//register stream objects.
			Assembly asmbs =  Assembly.GetExecutingAssembly ();
			foreach (Type curType in asmbs.GetTypes()) {
				try
				{
					if (_toLoadNamespaces.Contains (curType.Namespace)) {
						if (curType.IsClass) {
							MethodInfo methodinfo = curType.GetMethod("Type");
							if (methodinfo != null)
							{
								string  str = methodinfo.Invoke (Activator.CreateInstance(curType), null).ToString();
								RegisterStreamObj(	ushort.Parse(str), curType.Namespace+"."+curType.Name);
							}
						}
					}
				}
				catch(Exception e) {
					Debug.LogError (e.Message);
				}
			}
		}

		public void RegisterStreamObj(ushort objType, string typeName)
		{
			if (typeName == null) {
				Debug.LogError ("JGameManager.RegisterStreamObj empty  object type");
				return;
			}

			if (_typeToTypeName.ContainsKey (objType)) {
				Debug.LogError ("JGameManager.RegisterStreamObj registed  object type"+typeName);
				return;
			}

			_typeToTypeName [objType] = typeName;
		}

		public string GetStreamObjTypeName (ushort objType)
		{
			if (!_typeToTypeName.ContainsKey (objType)) {
				Debug.LogError ("JGameManager.RegisterStreamObj can not find object type name of type:"
					+JGameUtil.GetDescription((StreamObject.JObjectType)objType));
				return null;
			}

			return _typeToTypeName [objType];
		}
	}
}