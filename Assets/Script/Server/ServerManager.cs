using System;
using UnityEngine;
using UnityEngine.UI;
using JGame;
using JGame.Logic;

public class ServerManager : MonoBehaviour
{
	public Text ServerIP;
	public Text ServerPort;

	public static bool ServerActive = false;

	public void StartServer ()
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
		ServerActive = true;
		Debug.Log ("initialize finished");
	}
		
	void Start()
	{
	}
}

