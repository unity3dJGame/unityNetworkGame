using System;

namespace JGame
{
	using Data;
	using StreamObject;
	using Network;
	namespace Processer
	{
		
		public class JProcesserSignInServer :  IProcesser
		{
			public void run(IDataSet dataSet)
			{
				IStreamObj obj = dataSet.getData (JObjectType.sign_in);
				JObj_SignIn signInObj = obj as JObj_SignIn;
				if (signInObj == null)
					return;

				string account = signInObj._strAccount;
				string code = signInObj._strCode;
				JLog.Info("receive npt_signin_req packet from client: account:"+account+"  code:"+code, JGame.Log.JLogCategory.Network);

				JObj_SignRet resultObj = new JObj_SignRet ();
				//ToDo:if account and code is in database then
				if (account == "test" && code == "123") 
				{
					resultObj.Result = true;
				}
				else
				{
					resultObj.Result = false;
				}

				try {
					JNetworkDataOperator.SendData (JPacketType.npt_signin_ret, resultObj);
					JLog.Info("send npt_signin_ret packet to client", JGame.Log.JLogCategory.Network);
					return;
				} catch (Exception e) {
					JLog.Debug ("发送数据失败");
					JLog.Error (e.Message);
					return;
				}
			}
		}

	}
}