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
			GetType(pieceType);
			if(!canMove) {
				board.ChangeState(0);
			}
			board.SelectPiece(this.gameObject);
			board.SetisMoving(true);
			
		}
		if(board.GetState() == 1) {
			board.ChangeState(0);
		} 
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

	public bool FailCheck(int failsafes, int limit) {
		int i = 0;
		if(i < limit) {
			for(int j = 0; j < failsafes; ++j) {
				if(failSafe[j]) {
					i++;
					if(i == limit) {
						canMove = false;
					}
				}
			}
		}
		if(canMove) {
			return false;
		} else {
			return true;
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
										} else {
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
							if(board.GetColumn(MyPos[0] - 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant()) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
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
							FailCheck(failSafe.Length, failLimit); 
						}
					} else { // Black Player
						if(firstMove) { // Pawn could move two tiles forward
							// First Move - Movement
							failSafe = new bool[15];
							failLimit = 4;
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
									}else {
										failSafe[0] = true;
									}
								} else {
									failSafe[1] = true;
								}
							} else {
								failSafe[2] = true;
							}
							if(board.GetColumn(MyPos[0]) != null) {
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
										if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) {
												board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1);
												board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); 
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
							// Attack
							if(board.GetColumn(MyPos[0] + 1) != null) {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
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
							if(board.GetColumn(MyPos[0] - 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
										} else {
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
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
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
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
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
							if(board.GetColumn(MyPos[0] - 1) != null) {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) {
										if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) {
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2);
											board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true);
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
							FailCheck(failSafe.Length, failLimit); 
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
					failSafe = new bool[20];
					failLimit = 8;
					if(board.GetColumn(MyPos[0] + 1) != null) {
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true);
								} else {
									failSafe[0] = true;
								}
							}
						} else {
							failSafe[1] = true;
						}
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
								} else {
									failSafe[2] = true;
								}
							}
						} else {
							failSafe[3] = true;
						}
					} else {
						failSafe[4] = true;
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
								} else {
									failSafe[5] = true;
								}
							}
						} else {
							failSafe[6] = true;
						}
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true);
								} else {
									failSafe[7] = true;
								}
							}
						} else {
							failSafe[8] = true;
						}
					} else {
						failSafe[9] = true;
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
								} else {
									failSafe[10] = true;
								}
							}
						} else {
							failSafe[11] = true;
						}
						if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
							if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
								} else {
									failSafe[12] = true;
								}
							}
						} else {
							failSafe[13] = true;
						}
					} else {
						failSafe[14] = true;
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
								} else {
									failSafe[15] = true;
								}
							}
						} else {
							failSafe[16] = true;
						}
						if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
							if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) {
								board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1);
								board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
							} else {
								if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) {
									board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2);
									board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true);
								} else {
									failSafe[17] = true;
								}
							}
						} else {
							failSafe[18] = true;
						}
					} else {
						failSafe[19] = true;
					}
					FailCheck(failSafe.Length, failLimit); 
				}
			#endregion
			break;
			case 3:
			#region Bishop 
				// Bishop's Move
				if(board.GetState() == 0) { // Check if Neutral
					board.ChangeState(1); // Change State to Move
					failSafe = new bool[12];
					failLimit = 4;
					bool[] moveSafe = new bool[24];
					#region E1
					if(board.GetColumn(MyPos[0] + 1) != null) { // Check if the Right Side exist  
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {  // Northeast
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) { // Check if Unoccupied
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1); // Turns into Green
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
							} else { // if Occupied, What side occupies it?
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2); // Turns into Red
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									failSafe[3] = true;
								}
							}
						} else {
							failSafe[2] = true;
						}
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) { // Northwest
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) { // Check if Unoccupied
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1); // Turns into Green
								board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
							} else { // if Occupied, What side occupies it?
								if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2); // Turns into Red
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									failSafe[5] = true;
								}
							}
						} else {
							failSafe[4] = true;
						}
					} else { // if the Column doesn't exist
						failSafe[0] = true;
						failSafe[1] = true;
					}
					#endregion
					#region E2
					if(board.GetColumn(MyPos[0] + 2) != null && board.GetColumn(MyPos[0] + 1) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[0]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[0] = true;
								moveSafe[2] = true;
								moveSafe[4] = true;
								moveSafe[6] = true;
								moveSafe[8] = true;
								moveSafe[10] = true;
								
							}
						}
						if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
							if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[1]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[1] = true;
								moveSafe[3] = true;
								moveSafe[5] = true;
								moveSafe[7] = true;
								moveSafe[9] = true;
								moveSafe[11] = true;
							}
						}
					} 
					#endregion
					#region E3
					if(board.GetColumn(MyPos[0] + 3) != null && board.GetColumn(MyPos[0] + 2) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
							if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[2]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[2] = true;
								moveSafe[4] = true;
								moveSafe[6] = true;
								moveSafe[8] = true;
								moveSafe[10] = true;
							}
						}
						if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
							if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[3]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[3] = true;
								moveSafe[5] = true;
								moveSafe[7] = true;
								moveSafe[9] = true;
								moveSafe[11] = true;
							}
						}
					}
					#endregion
					#region E4
					if(board.GetColumn(MyPos[0] + 4) != null && board.GetColumn(MyPos[0] + 3) != null ) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) {
							if(!board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[4]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[4] = true;
								moveSafe[6] = true;
								moveSafe[8] = true;
								moveSafe[10] = true;
							}
						}
						if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) {
							if(!board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[5]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[5] = true;
								moveSafe[7] = true;
								moveSafe[9] = true;
								moveSafe[11] = true;
							}
						}
					}
					#endregion
					#region E5
					if(board.GetColumn(MyPos[0] + 5) != null && board.GetColumn(MyPos[0] + 4) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) {
							if(!board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[6]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[6] = true;
								moveSafe[8] = true;
								moveSafe[10] = true;
							}
						}
						if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) {
							if(!board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[7]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[7] = true;
								moveSafe[9] = true;
								moveSafe[11] = true;
							}
						}
					}
					#endregion
					#region E6
					if(board.GetColumn(MyPos[0] + 6) != null && board.GetColumn(MyPos[0] + 5) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) {
							if(!board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[8]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[8] = true;
								moveSafe[10] = true;
							}
						}
						if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) {
							if(!board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[9]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] -6 ) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[9] = true;
								moveSafe[11] = true;
							}
						}
					}
					#endregion
					#region E7
					if(board.GetColumn(MyPos[0] + 7) != null && board.GetColumn(MyPos[0] + 6) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) {
							if(!board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[10]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[10] = true;
							}
						}
						if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6) != null) {
							if(!board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[11]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[11] = true;
							}
						}
					}
					#endregion
					#region W1
					if(board.GetColumn(MyPos[0] - 1) != null) { // Check if the Right Side exist  
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {  // Northeast
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) { // Check if Unoccupied
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1); // Turns into Green
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
							} else { // if Occupied, What side occupies it?
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2); // Turns into Red
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									failSafe[9] = true;
								}
							}
						} else {
							failSafe[8] = true;
						}
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) { // Northwest
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) { // Check if Unoccupied
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1); // Turns into Green
								board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
							} else { // if Occupied, What side occupies it?
								if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2); // Turns into Red
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									failSafe[11] = true;
								}
							}
						} else {
							failSafe[10] = true;
						}
					} else { // if the Column doesn't exist
						failSafe[6] = true;
						failSafe[7] = true;
					}
					#endregion
					#region W2
					if(board.GetColumn(MyPos[0] - 2) != null && board.GetColumn(MyPos[0] - 1) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[12]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[12] = true;
								moveSafe[14] = true;
								moveSafe[16] = true;
								moveSafe[18] = true;
								moveSafe[20] = true;
								moveSafe[22] = true;
							}
						}
						if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
							if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[13]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[13] = true;
								moveSafe[15] = true;
								moveSafe[17] = true;
								moveSafe[19] = true;
								moveSafe[21] = true;
								moveSafe[23] = true;
							}
						}
					} 
					#endregion
					#region W3
					if(board.GetColumn(MyPos[0] - 3) != null && board.GetColumn(MyPos[0] - 2) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
							if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[14]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[14] = true;
								moveSafe[16] = true;
								moveSafe[18] = true;
								moveSafe[20] = true;
								moveSafe[22] = true;
							}
						}
						if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
							if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[15]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[15] = true;
								moveSafe[17] = true;
								moveSafe[19] = true;
								moveSafe[21] = true;
								moveSafe[23] = true;
							}
						}
					} 
					#endregion
					#region W4
					if(board.GetColumn(MyPos[0] - 4) != null && board.GetColumn(MyPos[0] - 3) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) {
							if(!board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[16]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[16] = true;
								moveSafe[18] = true;
								moveSafe[20] = true;
								moveSafe[22] = true;
							}
						}
						if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) {
							if(!board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[17]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[17] = true;
								moveSafe[19] = true;
								moveSafe[21] = true;
								moveSafe[23] = true;
							}
						}
					} 
					#endregion
					#region W5
					if(board.GetColumn(MyPos[0] - 5) != null && board.GetColumn(MyPos[0] - 4) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) {
							if(!board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[18]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[18] = true;
								moveSafe[20] = true;
								moveSafe[22] = true;
							}
						}
						if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) {
							if(!board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[19]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[19] = true;
								moveSafe[21] = true;
								moveSafe[23] = true;
							}
						}
					} 
					#endregion
					#region W6
					if(board.GetColumn(MyPos[0] - 6) != null && board.GetColumn(MyPos[0] - 5) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) {
							if(!board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[20]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[20] = true;
								moveSafe[22] = true;
							}
						}
						if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) {
							if(!board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[21]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[21] = true;
								moveSafe[23] = true;
							}
						}
					} 
					#endregion
					#region W7
					if(board.GetColumn(MyPos[0] - 7) != null && board.GetColumn(MyPos[0] - 6) != null) { // Check if the Right Side exist
						if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) {
							if(!board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[22]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // NE moveSafe
								moveSafe[22] = true;
							}
						}
						if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6) != null) {
							if(!board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetState()) { // Check previous Tile
								if(!moveSafe[23]) { // if Previous Tile are occupied
									if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7) != null) { // Northeast
										if(!board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().GetState()) { // Check if Unoccupied
											board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().ChangeColor(1); // Turns into Green
											board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
										} else { // if Occupied, What side occupies it?
											if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
												board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().ChangeColor(2); // Turns into Red
												board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
											} 
										}
									}
								}  
							} else { // SE moveSafe
								moveSafe[23] = true;
							}
						}
					} 
					#endregion
					FailCheck(failSafe.Length, failLimit);
				}
			#endregion
			break;
			case 4:
			#region Rook
				// Rook's Move
				if(board.GetState() == 0) { // Check if Neutral
					board.ChangeState(1); // Change State to Move
					failSafe = new bool[48];
					failLimit = 4;
					bool[] moveSafe = new bool[24];
					#region N&S
						if(board.GetColumn(MyPos[0]) != null) { // is the current Column Exist?
						#region N1
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) { // North + 1 exist?
								if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) { // Check if Unoccupied
									board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(1); // Turns into Green
									board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().ChangeColor(2); // Turns into Red
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
									} else {
										failSafe[3] = true;
									}
								}
							} else {
								failSafe[2] = true;
							}
						#endregion
						#region S1
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) { // North + 1 exist?
								if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) { // Check if Unoccupied
									board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(1); // Turns into Green
									board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().ChangeColor(2); // Turns into Red
										board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().SetisActive(true); // Can be Clicked
									} else {
										failSafe[5] = true;
									}
								}
							} else {
								failSafe[4] = true;
							}
						#endregion
						#region N2
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 1).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[0]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[0] = true;
										moveSafe[2] = true;
										moveSafe[4] = true;
										moveSafe[6] = true;
										moveSafe[8] = true;
										moveSafe[10] = true;
									}
								}
							} else {
								failSafe[6] = true;
							}
						#endregion
						#region S2
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 1).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[1]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[1] = true;
										moveSafe[3] = true;
										moveSafe[5] = true;
										moveSafe[7] = true;
										moveSafe[9] = true;
										moveSafe[11] = true;
									}
								}
							} else {
								failSafe[7] = true;
							}
						#endregion
						#region N3
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 2).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[2]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[2] = true;
										moveSafe[4] = true;
										moveSafe[6] = true;
										moveSafe[8] = true;
										moveSafe[10] = true;
									}
								}
							} else {
								failSafe[8] = true;
							}
						#endregion
						#region S3
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 2).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[3]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[3] = true;
										moveSafe[5] = true;
										moveSafe[7] = true;
										moveSafe[9] = true;
										moveSafe[11] = true;
									}
								}
							} else {
								failSafe[9] = true;
							}
						#endregion
						#region N4
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 3).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[4]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[4] = true;
										moveSafe[6] = true;
										moveSafe[8] = true;
										moveSafe[10] = true;
									}
								}
							} else {
								failSafe[10] = true;
							}
						#endregion
						#region S4
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 3).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[5]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[5] = true;
										moveSafe[7] = true;
										moveSafe[9] = true;
										moveSafe[11] = true;
									}
								}
							} else {
								failSafe[11] = true;
							}
						#endregion
						#region N5
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 4).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[6]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[6] = true;
										moveSafe[8] = true;
										moveSafe[10] = true;
									}
								}
							} else {
								failSafe[12] = true;
							}
						#endregion
						#region S5
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 4).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[7]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[7] = true;
										moveSafe[9] = true;
										moveSafe[11] = true;
									}
								}
							} else {
								failSafe[13] = true;
							}
						#endregion
						#region N6
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 5).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[8]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[8] = true;
										moveSafe[10] = true;
									}
								}
							} else {
								failSafe[14] = true;
							}
						#endregion
						#region S6
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 5).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[9]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[9] = true;
										moveSafe[11] = true;
									}
								}
							} else {
								failSafe[15] = true;
							}
						#endregion
						#region N7
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 6).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[10]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] + 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[10] = true;
									}
								}
							} else {
								failSafe[16] = true;
							}
						#endregion
						#region S7
							if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7) != null) { // North + 1 exist?
								if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6) != null) {
									if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 6).GetComponent<Tile>().GetState()) { // Check Precious Tile
										if(!moveSafe[11]) { // if the precious Tile are Occupied
											if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7) != null) { // Northeast
												if(!board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().GetState()) { // Check if Unoccupied
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().ChangeColor(1); // Turns into Green
													board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
												} else {
													if(board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().ChangeColor(2); // Turns into Red
														board.GetColumn(MyPos[0]).GetComponent<Column>().Tiles(MyPos[1] - 7).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} 
												}
											}
										}
									} else {
										moveSafe[11] = true;
									}
								}
							} else {
								failSafe[17] = true;
							}
						#endregion
						} else {
							failSafe[0] = true;
							failSafe[1] = true;
						}
					#endregion	
					#region East
						#region E1
						if(board.GetColumn(MyPos[0] - 1) != null) { // is the current Column - 1 Exist
							if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 1 exist?
								if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
									board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
										board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
										board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
									} else {
										failSafe[20] = true;
									}
								}
							} else {
								failSafe[19] = true;
							}
						} else {
							failSafe[18] = true;
						}
						#endregion
						#region E2
							if(board.GetColumn(MyPos[0] - 2) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] - 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[12]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[12] = true;
											moveSafe[14] = true;
											moveSafe[16] = true;
											moveSafe[18] = true;
											moveSafe[20] = true;
											moveSafe[22] = true;
										}
									}
								} else {
									failSafe[22] = true;
								}
							} else {
								failSafe[21] = true;
							}
							#endregion
						#region E3
							if(board.GetColumn(MyPos[0] - 3) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] - 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[14]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[14] = true;
											moveSafe[16] = true;
											moveSafe[18] = true;
											moveSafe[20] = true;
											moveSafe[22] = true;
										}
									}
								} else {
									failSafe[24] = true;
								}
							} else {
								failSafe[23] = true;
							}
						#endregion
						#region E4
							if(board.GetColumn(MyPos[0] - 4) != null) { // is the current Column - 4 Exist
								if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] - 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[16]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[16] = true;
											moveSafe[18] = true;
											moveSafe[20] = true;
											moveSafe[22] = true;
										}
									}
								} else {
									failSafe[26] = true;
								}
							} else {
								failSafe[25] = true;
							}
						#endregion
						#region E5
							if(board.GetColumn(MyPos[0] - 5) != null) { // is the current Column - 5 Exist
								if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] - 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[18]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[18] = true;
											moveSafe[20] = true;
											moveSafe[22] = true;
										}
									}
								} else {
									failSafe[28] = true;
								}
							} else {
								failSafe[27] = true;
							}
						#endregion
						#region E6
							if(board.GetColumn(MyPos[0] - 6) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] - 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[20]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[20] = true;
											moveSafe[22] = true;
										}
									}
								} else {
									failSafe[30] = true;
								}
							} else {
								failSafe[29] = true;
							}
						#endregion
						#region E7
							if(board.GetColumn(MyPos[0] - 7) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] - 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[20]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] - 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[22] = true;
										}
									}
								} else {
									failSafe[32] = true;
								}
							} else {
								failSafe[31] = true;
							}
						#endregion
					#endregion
					#region West
						#region W1
						if(board.GetColumn(MyPos[0] + 1) != null) { // is the current Column - 1 Exist
							if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 1 exist?
								if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
									board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
								} else {
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
										board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
										board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
									} else {
										failSafe[35] = true;
									}
								}
							} else {
								failSafe[34] = true;
							}
						} else {
							failSafe[33] = true;
						}
						#endregion
						#region W2
							if(board.GetColumn(MyPos[0] + 2) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] + 1).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[13]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[13] = true;
											moveSafe[15] = true;
											moveSafe[17] = true;
											moveSafe[19] = true;
											moveSafe[21] = true;
											moveSafe[23] = true;
										}
									}
								} else {
									failSafe[37] = true;
								}
							} else {
								failSafe[36] = true;
							}
							#endregion
						#region W3
							if(board.GetColumn(MyPos[0] + 3) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] + 2).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[15]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[15] = true;
											moveSafe[17] = true;
											moveSafe[19] = true;
											moveSafe[21] = true;
											moveSafe[23] = true;
										}
									}
								} else {
									failSafe[39] = true;
								}
							} else {
								failSafe[38] = true;
							}
						#endregion
						#region W4
							if(board.GetColumn(MyPos[0] + 4) != null) { // is the current Column - 4 Exist
								if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] + 3).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[17]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[17] = true;
											moveSafe[19] = true;
											moveSafe[21] = true;
											moveSafe[23] = true;
										}
									}
								} else {
									failSafe[41] = true;
								}
							} else {
								failSafe[40] = true;
							}
						#endregion
						#region W5
							if(board.GetColumn(MyPos[0] + 5) != null) { // is the current Column - 5 Exist
								if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] + 4).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[19]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[19] = true;
											moveSafe[21] = true;
											moveSafe[23] = true;
										}
									}
								} else {
									failSafe[43] = true;
								}
							} else {
								failSafe[42] = true;
							}
						#endregion
						#region W6
							if(board.GetColumn(MyPos[0] + 6) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] + 5).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[21]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[21] = true;
											moveSafe[23] = true;
										}
									}
								} else {
									failSafe[45] = true;
								}
							} else {
								failSafe[44] = true;
							}
						#endregion
						#region W7
							if(board.GetColumn(MyPos[0] + 7) != null) { // is the current Column - 2 Exist
								if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]) != null) { // North + 2 exist?
									if(board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]) != null) {
										if(!board.GetColumn(MyPos[0] + 6).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check Precious Tile
											if(!moveSafe[23]) { // if the precious Tile are Occupied
												if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]) != null) { // Northeast
													if(!board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetState()) { // Check if Unoccupied
														board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(1); // Turns into Green
														board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
													} else {
														if(board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().GetOccupant() != isWhite) { // Check if it's an Enemy
															board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().ChangeColor(2); // Turns into Red
															board.GetColumn(MyPos[0] + 7).GetComponent<Column>().Tiles(MyPos[1]).GetComponent<Tile>().SetisActive(true); // Can be Clicked
														} 
													}
												}
											}
										} else {
											moveSafe[23] = true;
										}
									}
								} else {
									failSafe[47] = true;
								}
							} else {
								failSafe[46] = true;
							}
						#endregion
					#endregion
					FailCheck(failSafe.Length, failLimit);
				}
			#endregion
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
 