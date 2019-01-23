using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// Public Variables 
	public Material[] color; 
	// 0 = Normal State;
	// 1 = Potential Move State
	// 2 = Potential Attack State
	public Board board;
	public int rowID;
	public int colID;
	public bool isOccupied = false;

	// Private Variables 
	
	private bool isActive = false;
	private GameObject selectPiece;
	private Vector3 desiredV3;
	void Start() {
		color[0] = GetComponent<Renderer>().material;
	}

	void Update() {
		if(board.GetState() == 0) {
			ChangeColor(0);
			SetisActive(false);
		}
	}

	void OnMouseDown() {
		if(isActive) {
			selectPiece = board.GetSelectedPiece();
			SetDesiredV3(new Vector3(gameObject.transform.position.x, selectPiece.transform.position.y, gameObject.transform.position.z));
			board.SelectTile(this.gameObject);
			if(selectPiece.GetComponent<Piece>().GetisFirst()) {
				selectPiece.GetComponent<Piece>().SetisFirst(false);
			}
			selectPiece.GetComponent<Piece>().SetID(colID, rowID);
			board.ChangeState(2);
		}
	}

	public void ChangeColor(int state) {
		GetComponent<Renderer>().material = color[state];
	}

	public bool GetState() {
		return isOccupied;
	}

	public bool GetisActive() {
		return isActive;
	}
	
	public bool GetSelectedPiece() {
		return selectPiece;
	}

	public Vector3 GetDesiredV3() {
		return desiredV3;
	}

	public void SetState(bool newVal) {
		isOccupied = newVal;
	}

	public void SetisActive(bool newVal) {
		isActive = newVal;
	}

	public void SetDesiredV3(Vector3 newVal) {
		desiredV3 = newVal;
	}
}
