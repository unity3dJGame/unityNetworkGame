using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace JGame
{
	using JGame.Log;
	using JGame.Logic;
	using JGame.Network;
	using JGame.Processer;

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

		public bool ShutDown()
		{
			try
			{
				JLog.Info("JGameManager.ShutDown:Shut down server and client socket manager ...", JLogCategory.Network);
				JServerSocketManager.SingleInstance.ShutDown ();
				JClientSocketManager.SingleInstance.ShutDown ();		
				JLog.Info("JGameManager.ShutDown:Shut down server and client socket manager finished.", JLogCategory.Network);
			}
			catch (Exception e) {
				JLog.Error ("JGameManager.ShutDown:" + e.Message);
				return false;
			}
			return true;
		}

		public void initialize(bool bServer, string serverIP, int serverPort)
		{
			_typeToTypeName = new Dictionary<ushort, string> ();
			_toLoadNamespaces = new HashSet<string> ();
			_toLoadNamespaces.Add ("JGame.StreamObject");
			JLogicHelper.IsServer = bServer;

			//initialize log system.
			JLog.Initialize ();

			//initialize server or client network manager
			if (bServer) {
				JServerSocketManager.SingleInstance.Initialize (serverIP, serverPort);
				JProcessorRegister.RegisterServerProcessor ();
			} else {
				JClientSocketManager.SingleInstance.Initialize (serverIP, serverPort);
			}

			//register stream objects.
			Assembly asmbs =  Assembly.GetExecutingAssembly ();
			foreach (Type curType in asmbs.GetTypes()) {
				try
				{
					//JLog.Info(curType.Namespace);
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
					JLog.Error (e.Message);
				}
			}

			JLog.Debug ("JGameManager initialize finished .");
		}

		public void RegisterStreamObj(ushort objType, string typeName)
		{
			if (typeName == null) {
				JLog.Error("JGameManager.RegisterStreamObj empty  object type");
				return;
			}

			if (_typeToTypeName.ContainsKey (objType)) {
				JLog.Error("JGameManager.RegisterStreamObj registed  object type"+typeName);
				return;
			}

			_typeToTypeName [objType] = typeName;
		}

		public string GetStreamObjTypeName (ushort objType)
		{
			if (!_typeToTypeName.ContainsKey (objType)) {
				JLog.Error("JGameManager.RegisterStreamObj can not find object type name of type:"
					+JGameUtil.GetDescription((StreamObject.JObjectType)objType));
				return null;
			}

			return _typeToTypeName [objType];
		}
	}
}