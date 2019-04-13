using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("MySide", 0);
	}
	
	public void StartAsWhite() {
		PlayerPrefs.SetInt("MySide", 0);
		SceneManager.LoadScene("Board", LoadSceneMode.Single);
	}

	public void StartAsBlack() {
		PlayerPrefs.SetInt("MySide", 1);
		SceneManager.LoadScene("Board", LoadSceneMode.Single);
	}
}
