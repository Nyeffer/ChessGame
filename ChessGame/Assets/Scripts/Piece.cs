﻿using System.Collections;
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
	private int[] MyPast;
	private bool firstMove = true;
	private Vector3 pos;
	private int state = 0;
	private bool canMove = true;
	private bool[] failSafe;
	private int failLimit = 0;
	
	void Awake() {
		MyPos = new int[2];
		MyPos[0] = initCol;
		MyPos[1] = initRow;
		MyPast = new int[2];
		MyPast[0] = initCol;
		MyPast[1] = initRow;
	}

	void Start() {
		Move(new Vector3(board.GetColumn(MyPos[0]).GetComponent<Column>().GetTile(MyPos[1]).x, 
						gameObject.transform.position.y, 
						board.GetColumn(MyPos[0]).GetComponent<Column>().GetTile(MyPos[1]).z));
	}

	void Update() {
		state = board.GetState();
		if(MyPast[0] != MyPos[0] || MyPast[1] != MyPos[1]) {
			board.GetColumn(MyPast[0]).GetComponent<Column>().Tiles(MyPast[1]).GetComponent<Tile>().SetState(false);
			MyPast[0] = MyPos[0];
			MyPast[1] = MyPos[1];
			if(isWhite) {
				board.GetColumn(MyPast[0]).GetComponent<Column>().Tiles(MyPast[1]).GetComponent<Tile>().SetOccupant(isWhite);
			}
		} else {
			board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetState(true);
			if(isWhite) {
				board.GetColumn(MyPast[0]).GetComponent<Column>().Tiles(MyPast[1]).GetComponent<Tile>().SetOccupant(isWhite);
			}
		}
	}

	public void Move(Vector3 newVal) {
		gameObject.transform.position = new Vector3(newVal.x, gameObject.transform.position.y, newVal.z);
	}

	public void SetID(int col, int row) {
		MyPos[0] = col;
		MyPos[1] = row;
	}

	void OnMouseDown() {
		if(board.GetState() == 0) {
			canMove = true;
			board.SelectPiece(this.gameObject);
			board.SetisMoving(true);
			GetType(pieceType);
			if(!canMove) {
				board.ChangeState(0);
			}
		}
		Debug.Log(canMove);
		
	}

	public bool GetisFirst() {
		return firstMove;
	}

	public bool GetLegion() {
		return isWhite;
	}

	public void SetisFirst(bool newVal) {
		firstMove = newVal;
	}

	public void FailCheck(int failsafes, int limit) {
		int i = 0;
		if(i < limit) {
			Debug.Log(limit);
			for(int j = 0; j < failsafes; ++j) {
				if(failSafe[j]) {
					i++;
					if(i == limit) {
						canMove = false;
					}
				}
			}
		}
	}

	

	void GetType(int myType) {
		switch(myType) {
			case 1:
			#region Pawn
				// Pawn it's Moves
				if(board.GetState() == 0) { // Check if your Neutral
					board.ChangeState(1); // Change it to Move
					if(isWhite) { // White Player
						if(firstMove) { // Pawn could move two tiles forward
							failSafe = new bool[15];
							failLimit = 4;
							// First Move - Movement
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
									} else {
										failSafe[0] = true;
									}
								} else {
									failSafe[1] = true;
								}
							} else {
								failSafe[2] = true;
							}
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) {
												board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1);
												board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); 
										} else {
											failSafe[3] = true;
										}
									} else {
										failSafe[4] = true;
									}
								} else {
									failSafe[5] = true;
								}
							} else {
								failSafe[6] = true;
							} 
							// Attack
							if(board.GetColumn(MyPos[0] + 1) != null) {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										} else {
											failSafe[7] = true;
										}
									} else {
										failSafe[8] = true;
									}
								} else {
									failSafe[9] = true;
								}
							} else {
								failSafe[10] = true;
							} 
							if(board.GetColumn(MyPos[0] - 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										}else {
											failSafe[11] = true;
										}
									} else {
										failSafe[12] = true;
									}
								} else {
									failSafe[13] = true;
								}
							} else {
								failSafe[14] = true;
							}
							FailCheck(failSafe.Length, failLimit);  
						} else {
							// Non-First Move - Movement
							failSafe = new bool[11];
							failLimit = 3;
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
									} else {
										failSafe[0] = true;
									}
								} else {
									failSafe[1] = true;
								}
							} else {
								failSafe[2] = true;
							}
							// Attack
							if(board.GetColumn(MyPos[0] + 1) != null) {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant()) {
										if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										}else {
											failSafe[3] = true;
										}
									} else {
										failSafe[4] = true;
									}
								} else {
									failSafe[5] = true;
								}
							} else {
								failSafe[6] = true;
							} 
							if(board.GetColumn(MyPos[0] + 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant()) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										}else {
											failSafe[7] = true;
										}
									} else {
										failSafe[8] = true;
									}
								} else {
									failSafe[9] = true;
								}
							} else {
								failSafe[10] = true;
							} 
							FailCheck(failSafe.Length, failLimit); 
						}
					} else { // Black Player
						if(firstMove) { // Pawn could move two tiles forward
							// First Move - Movement
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
									}
								}
							}
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
									}
								}
							}
							// Attack
							if(board.GetColumn(MyPos[0] + 1) != null) {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										}
									}
								}
							}
							if(board.GetColumn(MyPos[0] - 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										}
									}
								}
							} 
						} else {
							// Non-First Move - Movement
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
									}
								}
							}
							// Attack
							if(board.GetColumn(MyPos[0] + 1) != null) {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										}
									}
								}
							}
							if(board.GetColumn(MyPos[0] - 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										} 
									} 
								} 
							} else {
								canMove = false;
							}
						}
					}
				}
			#endregion
			break;
			case 2:
			#region Knight
				// Knight's Move
				if(board.GetState() == 0) { // Check if your Neutral
					board.ChangeState(1); // Change to Move
					if(board.GetColumn(MyPos[0] + 1) != null) {
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true);
								}
							}
						}
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
								}
							}
						}
					}
					if(board.GetColumn(MyPos[0] - 1) != null) {
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true);
								}
							}
						}
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
								}
							}
						}
					}
					if(board.GetColumn(MyPos[0] + 2) != null) {
						if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
							if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
								}
							}
							if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
								if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
									board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
									board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
								} else {
									if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2);
										board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
									}
								}
							}
						}
					}
					if(board.GetColumn(MyPos[0] - 2) != null) {
						if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
							if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
								}
							}
						}
						if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
							if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
								}
							}
						}
					} 
				}
			#endregion
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
