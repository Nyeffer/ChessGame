using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	// Public Variables
	public int pieceType = 0; // ID for each Piece
	// 0 = None
	// 1 = Pawn
	// 2 = Horse
	// 3 = Bishop
	// 4 = Rock
	// 5 = Queen
	// 6 = King
	public Board board; // Instance of the Board
	public int initCol = 0;
	public int initRow = 0;

	// Private Variables
	private int[] MyPos;
	private Vector3 pos;
	void Start() {
		MyPos = new int[2];
		MyPos[0] = initCol;
		MyPos[1] = initRow;
	}

	void Update() {
		
	}

	void Move() {
		gameObject.transform.position = new Vector3(board.GetColumn(MyPos[0]).GetComponent<Column>().GetTile(MyPos[1]).x,
													gameObject.transform.position.y,
													board.GetColumn(MyPos[0]).GetComponent<Column>().GetTile(MyPos[1]).z);
	}
}
