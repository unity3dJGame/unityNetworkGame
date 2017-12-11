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
			public override void run(IDataSet dataSet)
			{
				IStreamObj obj = dataSet.getData (JObjectType.sign_in);
				JObj_SignIn signInObj = obj as JObj_SignIn;
				if (signInObj == null)
					return;

				string account = signInObj._strAccount;
				string code = signInObj._strCode;
				//ToDo:if account and code is in database then
				if (true) {
					JObj_SignRet resultObj = new JObj_SignRet();
					resultObj.Result = true;
					try {
						JNetworkDataOperator.SendData(JPacketType.npt_signin_ret, resultObj);
						return true;
					} catch (Exception e) {
						JLog.Debug ("发送数据失败");
						JLog.Error (e.Message);
						return false;
					}
				}
			}
		}

	}
}