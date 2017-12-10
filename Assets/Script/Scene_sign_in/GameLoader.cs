using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame;

public class GameLoader : MonoBehaviour {
	public bool IsServer = false;
	public string ServerIP = "192.168.1.101";
	public int ServerPort = 9796;

	// Use this for initialization
	void Start () {
		
		InitializeManagers ();


	}
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	public void InitializeManagers()
	{
		JGame.JGameManager.SingleInstance.initialize (IsServer, ServerIP, ServerPort);
	}
}
