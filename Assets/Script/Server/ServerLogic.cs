using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Logic;

public class ServerLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ServerManager.ServerActive)
			JLogic.Logic ();
	}
}
