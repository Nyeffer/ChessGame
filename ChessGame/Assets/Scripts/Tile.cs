using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// Public Variables 
	public Material[] color; 
	// 0 = Normal State;
	// 1 = Potential Move State
	// 2 = Potential Attack State

	// Private Variables 
	private bool isOccupied = false;
	void Start() {
		color[0] = GetComponent<Renderer>().material;
	}

	public void ChangeColor(int state) {
		GetComponent<Renderer>().material = color[state];
	}

	public bool GetState() {
		return isOccupied;
	}

}
