using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

	// Public Variables 
	public GameObject[] tiles;
	// Private Variables
	private Vector3[] pos;

	// Initialize Constructors
	void Start() {
		pos = new Vector3[tiles.Length];
		for (int i = 0; i < tiles.Length; i++) {
			pos[i] = tiles[i].transform.position;
		}
	}

	// Get the position or Vector3 of a specific tile of this Column
	public Vector3 GetTile(int tileNum) {
		Vector3 tile = new Vector3();
		if(tileNum < tiles.Length && tileNum > -1) {
			tile = pos[tileNum];
		}
		return tile;
	}

	public GameObject Tiles(int tileID) {
		return tiles[tileID];
	}


}
