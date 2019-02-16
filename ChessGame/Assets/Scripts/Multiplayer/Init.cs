using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("MySide", 0);
	}

	public void IamWhite() {
		PlayerPrefs.SetInt("MySide", 0);
	}

	public void IamBlack() {
		PlayerPrefs.SetInt("MySide", 1);
	}
}
