using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

	// Public Varaibles
	public Transform[] views;
	public float transSpeed;
	Transform curView;
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.Lerp(transform.position, curView.position, Time.deltaTime * transSpeed);
	}

	public void GoToWhite() {
		curView = views[0];
	}

	public void GoToBlack() {
		curView = views[1];
	}

}
