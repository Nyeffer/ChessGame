using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	// Public Varaibles
	public GameObject[] col;
	public int[] tilesID;
	// Private Variables

	void Start() {
		Destroy(col[tilesID[0]].GetComponent<Column>().Tiles(tilesID[1]));
	}
}
