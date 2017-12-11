using System;
using UnityEngine;
using JGame;

public class ServerShutDown : MonoBehaviour
{
	public void ShutDownServer()
	{
		if (JGameManager.SingleInstance.ShutDown ()) {
			Debug.Log ("Server shut down : finished");
		} else {
			Debug.Log ("Server shut down : falied");
		}
	}
}


