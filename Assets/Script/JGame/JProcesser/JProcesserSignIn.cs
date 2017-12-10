using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace JGame
{
	using StreamObject;
	using Network;
	using Data;
	using Logic;
	using JGame.Data;

	namespace Processer
	{
		
		public class JProcesserSignIn : IProcesser
		{

			protected JProcesserSignIn() {}

			public virtual void run(IDataSet dataSet)
			{
			}

			#region 私有方法

			protected bool SendToServer(JObj_SignIn obj)
			{
				try {
					JNetworkDataOperator.SendData(JPacketType.npt_signin_req, obj);
					return true;
				} catch (Exception e) {
					JLog.Debug ("发送数据失败");
					JLog.Error (e.Message);
					return false;
				}
			}
			#endregion
		}

		public class JProcesserSignInSet :  JProcesserSignIn
		{
			public override void run(IDataSet dataSet)
			{
				IStreamObj obj = dataSet.getData (JObjectType.sign_in);
				JObj_SignIn signInObj = obj as JObj_SignIn;
				if (signInObj == null)
					return;
				SendToServer (signInObj);
			}
		}

		public class JProcesserSignInGet: JProcesserSignIn
		{
			public override void run(IDataSet dataSet)
			{
				IStreamObj obj = dataSet.getData (JObjectType.sign_in_ret);

				//todo:...

			}
		}
	}
}

