using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class progress_init : MonoBehaviour {

	// Use this for initialization
	void Start () {

		RectTransform progress = GetComponent<RectTransform> ();
		if (progress != null) {
			progress.anchoredPosition3D = new Vector3(0, progress.anchoredPosition3D.y, progress.anchoredPosition3D.z);
		
			progress.sizeDelta = new Vector2 (Screen.width, progress.sizeDelta.y);
		}

	}

}
