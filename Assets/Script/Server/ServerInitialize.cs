using System;
using UnityEngine;
using UnityEngine.UI;
using JGame;

public class ServerInitialize : MonoBehaviour
{
	public Text ServerIP;
	public Text ServerPort;
	public void Initialize ()
	{
		if (null == ServerIP) {
			Debug.Log ("ServerIP is null");
			return;
		}
		if (null == ServerPort) {
			Debug.Log ("ServerPort is null");
			return;
		}

		Debug.Log ("Server IP:" + ServerIP.text + "   ServerPort:" + ServerPort.text);
		JGameManager.SingleInstance.initialize (true, ServerIP.text, int.Parse (ServerPort.text));
		Debug.Log ("initialize finished");
	}
}

