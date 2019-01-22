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
	public bool isWhite = true;

	// Private Variables
	private int[] MyPos;
	private bool firstMove = true;
	private Vector3 pos;
	private int state = 0;
	// 0 = Neutral
	// 1 = Comfirm
	// 2 = Move
	// 3 = Fight
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

	void OnMouseDown() {
		GetState(state);
	}

	void GetState(int myState) {
		switch(myState) {
			case 0:
				// Neutral, when press on this state the select will show it's potential Moves
			break;
			case 1:
				// Comfirm, after selecting a Tile. The piece confirm if it's Fight or Move
			break;
			case 2:
				// Move, if the tile is Empty. Simply Move to that Vector3
			break;
			case 3:
				// Fight, if the tile is Occupied. a Sequence will play
			break;
			default:
			break;
		}
	}

	void GetType(int myType) {
		switch(myType) {
			case 1:
				GameObject[] potentialMoves = new GameObject[4];
				// Pawn it's Moves
				if(isWhite) {
					if(firstMove) {
						board.GetColumn(MyPos[0] + 1).GetComponent<>;
					}
				}
			break;
			case 2:
			break;
			case 3:
			break;
			case 4:
			break;
			case 5:
			break;
			case 6:
			break;
			default:
			break;
		}
	}
}
