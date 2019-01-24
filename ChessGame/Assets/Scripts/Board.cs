using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	// Public Varaibles
	public GameObject[] col;
	public int[] tilesID;
	public float timeOfAction = 1.0f;
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

	void Start() {
		// Destroy(col[tilesID[0]].GetComponent<Column>().Tiles(tilesID[1]));
	}

	void Update() {
		if(state == 2) {
			if(counter / timeOfAction <= 1) {
				Debug.Log(counter / timeOfAction);
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
				ChangeState(0);
			}
		}
	}

	public GameObject GetColumn(int ColID) {
		return col[ColID];
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


	


}
