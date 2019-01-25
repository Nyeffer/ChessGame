using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

	// Public Variables 
	public GameObject[] tiles;
	
	// Private Variables
	private Vector3[] pos;



	// Get the position or Vector3 of a specific tile of this Column
	public Vector3 GetTile(int tileNum) {
		Vector3 tile = new Vector3();
		if(tileNum < tiles.Length && tileNum > -1) {
			tile = tiles[tileNum].transform.position;
		} else {
			return Vector3.zero;
		}
		return tile;
	}



	public GameObject Tiles(int tileID) {
		if(tileID > -1 && tileID < tiles.Length) {
			return tiles[tileID];
		} else {
			return null;
		}
	}


}
