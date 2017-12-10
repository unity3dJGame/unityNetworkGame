using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using JGame;
using JGame.StreamObject;
using JGame.Network;
using JGame.Data;
using JGame.Processer;
using JGame.LocalData;
using JGame.Logic;

public class SignInNet : MonoBehaviour {
	public Text _user_account;		//用户账号
	public Text _user_code;			//用户密码
	public Slider _progress = null; //进度条
	public Text _progress_text = null;//进度条文字

	private static string _server_ip = "127.0.0.1";
	private static int	  _server_port = 9796;	
	private static Socket _client_socket = null;
	private static bool   _connected = false;

	// Use this for initialization
	void Start () {


	}
	void Update(){
		JLogic.Logic ();
	}

	//登录检查
	public void CheckToSignIn ()
	{
		JObj_SignIn obj = new JObj_SignIn();
		obj._strAccount = _user_account.text;
		obj._strCode = _user_code.text;
	
		JLocalDataHelper.addData (JPacketType.npt_signin_req, obj);
	}

	//注册
	public void Test_registerUser()
	{
		JObj_SignRet obj = new JObj_SignRet();
		obj.Result = true;
		JLocalDataHelper.addData (JPacketType.npt_signin_ret, obj);

	}

	public bool SendToServer(JObj_SignIn obj)
	{
		if (!_connected)
			return false;

		try
		{

			JOutputStream jstream = new JOutputStream();
			JBinaryReaderWriter.Write(ref jstream,  obj);
			_client_socket.Send(jstream.ToArray());
		}
		catch (Exception e) {
			Debug.Log ("发送数据失败");
			Debug.LogError (e.Message);
			return false;
		}

		int nReceivedCount = 0;
		byte[] buffer = new byte[JTcpDefines.max_buffer_size];
		do {
			nReceivedCount = _client_socket.Receive (buffer, JTcpDefines.max_buffer_size, SocketFlags.None);
		} while(nReceivedCount == 0);

		//...received packet
		JInputStream inputStream = new JInputStream(buffer);
		IStreamObj receivedObj = JBinaryReaderWriter.Read<IStreamObj>(inputStream);
		ushort utype = receivedObj.Type();

		return true;
	}


	// Use this for sign in
	public void SignIn () {
		Debug.Log ("start game button is clicked.");

		/*if (!Check (user_account.text, user_code.text)) {
			Debug.Log ("user name or code is not right.");
			return;
		}*/

		StartCoroutine (SwitchScene ("select_player"));

		Debug.Log ("Welcome!");
	}
		
	public  IEnumerator SwitchScene (string strSceneName)
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

}
