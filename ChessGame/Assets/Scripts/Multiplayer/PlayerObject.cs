using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour {

	public GameObject PlayerUnitPlayer;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer == false) {
			// Not mine
			return;
		}
		// Instantiate(PlayerUnitPlayer);
		CmdSpawnMyUnit();
	}
	
	// Update is called once per frame
	void Update () {

		
	}
	[Command]
	void CmdSpawnMyUnit() {
		GameObject go = Instantiate(PlayerUnitPlayer);
		NetworkServer.Spawn(go);
	}
}
