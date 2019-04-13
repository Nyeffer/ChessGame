using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public bool isWhite = true;
	public Board board;
	
	void Start() {
		board = GameManager.instance.GetBoard();
		if(isWhite) {
			if(board.GetWhite() == null) {
				board.SetWhite(gameObject.GetComponent<Player>());
			} else {
				isWhite = false;
				board.SetBlack(gameObject.GetComponent<Player>());
			}
		} else {
			if(board.GetBlack() == null) {
				board.SetBlack(gameObject.GetComponent<Player>());
			} else {
				isWhite = true;
				board.SetWhite(gameObject.GetComponent<Player>());
			}
		}

		if(PlayerPrefs.GetInt("MySide", 0) == 0) {
			board.SetisMoving(true);
			for(int i = 0; i < 8; i++) {
				if(isLocalPlayer == false) {
					return;
				}
				GetComponent<SpawnUnits>().CmdSpawnPawn(board.piece[0].gameObject, 1, i, this.gameObject, 1);
			}
		} else {
			board.SetisMoving(false);
			for(int i = 0; i < 8; i++) {
				if(isLocalPlayer == false) {
					return;
				}
				GetComponent<SpawnUnits>().CmdSpawnPawn(board.piece[1].gameObject, 6, i, this.gameObject, 1);
			}
		}
	}

	void Update() {
		Debug.Log(isWhite);
	}
}
