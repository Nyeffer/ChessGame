﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	// Public Varaibles
	public GameObject[] col;
	private Player White;
	private Player Black;
	public int[] tilesID;
	public float timeOfAction = 1.0f;
	public Camera players;
	public Piece[] piece;
	// Private Variables
	private int state = 0;
	// 0 = Neutral
	// 1 = Comfirm
	// 2 = Move
	// 3 = Fight
	private GameObject selectedPiece;
	private GameObject selectedTile;
	private float counter = 0.0f;
	private float startTime;
	private float journeyLength;
	private bool isMoving = false;
	// Asking who's moving
	// True = White
	// False = Black
	private bool currentlyMoving = true;

	void Awake() {
		piece[0].SetBoard(GetComponent<Board>());
		piece[1].SetBoard(GetComponent<Board>());

	}

	void Start() {
		// Destroy(col[tilesID[0]].GetComponent<Column>().Tiles(tilesID[1]));
		
		
		
	}

	void Update() {
		if(state == 2) {
			if(counter / timeOfAction <= 1) {
				counter += Time.deltaTime;
				float disCovered = (Time.time - startTime) * timeOfAction;
				float fracJourney = disCovered / journeyLength;
				selectedPiece.transform.position = Vector3.Lerp(selectedPiece.transform.position, 
																selectedTile.GetComponent<Tile>().GetDesiredV3(), 
																counter / timeOfAction);
				SetisMoving(false);
			} else {
				selectedTile.GetComponent<Tile>().SetState(true);
				counter = 0.0f;
				if(currentlyMoving) {
					currentlyMoving = false;
					players.GetComponent<CamMovement>().GoToBlack();
					ChangeState(0);
				} else {
					currentlyMoving = true;
					players.GetComponent<CamMovement>().GoToWhite();
					ChangeState(0);
				}
			}
		}
	}

	public GameObject GetColumn(int ColID) {
		if(ColID < col.Length && ColID > -1) {
			return col[ColID];
		} else {
			return null;
		}
		
	}

	public int GetState() {
		return state;
	}

	public bool GetisMoving() {
		return isMoving;
	}

	public void ChangeState(int newState) {
		state = newState;
		if(state == 2) {
			startTime = Time.time;
			journeyLength = Vector3.Distance(selectedPiece.transform.position, 
											selectedTile.GetComponent<Tile>().GetDesiredV3());
		}
	}

	public GameObject GetSelectedPiece() {
		return selectedPiece;
	}

	public void SelectPiece(GameObject go) {
		selectedPiece = go;
	}

	public void SelectTile(GameObject v3) {
		selectedTile = v3;
	}

	public void SetisMoving(bool newVal) {
		isMoving = newVal;
	}

	public bool GetPlayer() {
		return currentlyMoving;
	}

	public void SetPlayer(bool player) {
		currentlyMoving = player;
		
	}

	public void SetWhite(Player player) {
		
		White = player;
	}

	public Player GetWhite() {
		return White;
	}

	public void SetBlack(Player player) {
		
		Black = player;
	}

	public Player GetBlack() {
		return Black;
	}


	


}
