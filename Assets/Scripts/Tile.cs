using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public delegate void OnTile();
	public OnTile onTile;
	public Tile previous, next;
	public bool up = false;
	public static bool tiled = false;

	[System.Serializable]
	public enum KeyAction {
		PRESS, HOLD
	}
	public KeyAction keyAction;

	void OnTriggerStay(Collider col) {
		if(col.tag == "PlayerCharacter" ) Mark();
		if(col.tag == "Detector") Player.nextTile = this;

		if(col.tag == "Player") {
			if(NoteHold()) {
				Player.ColorUI();
				BeatGenerator.SetHit();
			}
			if(TileTriggered() && !tiled) {
				if(onTile != null) onTile.Invoke();
				tiled = true;
			}
		}
	}

	protected void Mark() {
		Player.currentTile = this;
		GetComponent<MeshRenderer>().material.color = new Color(0.25f, 0.58f, 0.13f);
	}

	public bool TileTriggered() {
		return NoteHit() || NoteHold();
	}

	public bool NoteHit() {
		return Level.GetPlayer().NoteHit() && keyAction == KeyAction.PRESS;
	}

	public bool NoteHold() {
		return keyAction == KeyAction.HOLD && Input.GetKey(Player.GetKey());
	}
}