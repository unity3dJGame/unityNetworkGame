using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using JGame.StreamObject;
using JGame;
using JGame.Network;
using JGame.Data;
using JGame.Logic;
using JGame.Data;
using JGame.Processer;
using JGame.Log;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JProcesserSignIn :  MonoBehaviour
{
	public Text _user_account;		//用户账号
	public  Text _user_code;			//用户密码
	public Slider _progress = null; //进度条
	public Text _progress_text = null;//进度条文字

	void Start()
	{
		JLogicHelper.registerProcessor (JPacketType.npt_signin_req, new JProcesserSignInSet (), false);
		JProcesserSignInGet processor = new JProcesserSignInGet ();
		processor.toSignIn += SignIn;
		JLogicHelper.registerProcessor (JPacketType.npt_signin_ret,  processor,false);
	}
		
	// Use this for sign in
	public void SignIn () {
		Debug.Log ("start game button is clicked.");
		StartCoroutine( SwitchScene ("select_player"));
		Debug.Log ("Welcome!");
	}

	public IEnumerator SwitchScene (string strSceneName)
	{
		AsyncOperation aop = SceneManager.LoadSceneAsync (strSceneName);
		//aop.allowSceneActivation = false;

		_progress.gameObject.SetActive (true);

		while (aop.progress < 1.0f) {
			_progress.value = aop.progress;
			_progress_text.text = (aop.progress*100).ToString()+"%";
			yield return new WaitForEndOfFrame ();
		}

		_progress.GetComponent<Slider> ().value = 1.0f;
		//aop.allowSceneActivation = true;

		//yield retun aop;
	}



	public class JInnerProcesserSignIn : IProcesser
	{

		protected JInnerProcesserSignIn() {}

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

	public class JProcesserSignInSet :  JInnerProcesserSignIn
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

	public class JProcesserSignInGet: JInnerProcesserSignIn
	{
		public delegate void ToSignIn();
		public ToSignIn toSignIn;

		public override void run(IDataSet dataSet)
		{
			IStreamObj obj = dataSet.getData (JObjectType.sign_in_ret);
			if (null == obj || null == (obj as JObj_SignRet))
				JLog.Error ("JProcesserSignInGet : obj is empty!");

			if ((obj as JObj_SignRet).Result == true)

				//todo:...
			if (null != toSignIn)
				toSignIn ();
		}
	}

}

