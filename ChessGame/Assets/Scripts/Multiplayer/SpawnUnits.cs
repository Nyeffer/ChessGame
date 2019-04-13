using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnUnits : NetworkBehaviour {

	private Board board;

	void Start() {
		board = GameManager.instance.GetBoard();
	}

	[Command]
	public void CmdSpawnPawn(GameObject piece, int row, int col, GameObject game, int PieceType) {
		piece.GetComponent<Piece>().SetBoard(board);
		piece.GetComponent<Piece>().pieceType = PieceType;
		piece.GetComponent<Piece>().initCol = col; // Columns 1-8
		piece.GetComponent<Piece>().initRow = row; // 2nd Row or 7th Row
		NetworkServer.Spawn(Instantiate(piece));
	}
}
