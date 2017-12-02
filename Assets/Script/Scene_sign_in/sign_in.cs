using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using System.Data;


public class sign_in : MonoBehaviour {
	public Text text_edit_username = null;
	public Text text_edit_code = null;
	public Slider _progress = null;
	public Text _progress_text = null;

	private MySqlAccess _db = null;

	void Start(){
		_db = new MySqlAccess ();
	}

	// Use this for sign in
	public void SignIn () {
		Debug.Log ("start game button is clicked.");

		if (!Check (text_edit_username.text, text_edit_code.text)) {
			Debug.Log ("user name or code is not right.");
			return;
		}

		StartCoroutine (SwitchScene ("select_player"));

		Debug.Log ("Welcome!");
	}

	//Check user name and code are right or not
	public bool Check(string strName, string strCode){
		
		bool bRet = MySqlFunction.QueryUserIdAndCodeMatched (_db, strName, strCode);
		if (!bRet) {
			text_edit_username.GetComponentInParent<InputField>().text = "";
			text_edit_code.GetComponentInParent<InputField>().text = "";
		}

		return bRet;
	}

	private IEnumerator SwitchScene (string strSceneName)
	{
		AsyncOperation aop = Application.LoadLevelAsync (strSceneName);
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
