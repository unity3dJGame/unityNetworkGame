using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame;

public class GameLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		InitializeManagers ();

	}
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	public void InitializeManagers()
	{
		JGame.JGameManager.SingleInstance.initialize ();
	}
}
