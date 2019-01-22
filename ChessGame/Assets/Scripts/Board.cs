using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	// Public Varaibles
	public GameObject[] col;
	public int[] tilesID;
	// Private Variables
	private int state = 0;
	// 0 = Neutral
	// 1 = Comfirm
	// 2 = Move
	// 3 = Fight

	void Start() {
		// Destroy(col[tilesID[0]].GetComponent<Column>().Tiles(tilesID[1]));
	}

	public GameObject GetColumn(int ColID) {
		return col[ColID];
	}

	public int GetState() {
		return state;
	}

	public void ChangeState(int newState) {
		state = newState;
	}
}
