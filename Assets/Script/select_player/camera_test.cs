using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class camera_test : MonoBehaviour {
	public Camera main_camera;
	public float speed_move = 10;
	public float speed_rotate = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float dDeltaX = Input.GetAxis ("Horizontal")*speed_move*Time.deltaTime;
		float dDeltaY = Input.GetAxis ("Vertical") * speed_rotate * Time.deltaTime;
		float dRotateY = 0.0f;
		if (Input.GetKey (KeyCode.LeftShift)) {
			dRotateY = dDeltaX*2.0f;
		} else {
			main_camera.transform.Translate (dDeltaX, 0f, 0f);
		}
		main_camera.transform.Rotate (new Vector3(-dDeltaY, dRotateY, 0));
	}
}
