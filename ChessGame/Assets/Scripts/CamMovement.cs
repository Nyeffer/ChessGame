using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {

	// Public Varaibles
	public Transform[] views;
	public float transSpeed;
	Transform curView;

	void Start() {
		curView = transform;
	}
	
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.Lerp(transform.position, curView.position, Time.deltaTime * transSpeed);
		Vector3 curAngle = new Vector3(Mathf.LerpAngle(transform.rotation.eulerAngles.x, curView.rotation.eulerAngles.x, Time.deltaTime * transSpeed), 
										Mathf.LerpAngle(transform.rotation.eulerAngles.y, curView.rotation.eulerAngles.y, Time.deltaTime * transSpeed),
										Mathf.LerpAngle(transform.rotation.eulerAngles.z, curView.rotation.eulerAngles.z, Time.deltaTime * transSpeed));
		transform.eulerAngles = curAngle;
	}

	public void GoToWhite() {
		curView = views[0];
	}

	public void GoToBlack() {
		curView = views[1];
	}

}
