using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign_in_table_next : MonoBehaviour {
	
	public InputField ipf_name = null;
	public InputField ipf_code = null;
	public Button btn_sign_in = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Tab))
		{
			if (ipf_name.isFocused ) 
			{
				ipf_code.ActivateInputField ();
			} else if (ipf_code.isFocused) 
			{
				btn_sign_in.Select ();
			}
		}
	}
}
