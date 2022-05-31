using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect;
using UnityEngine.Events;

public class MenuCam : MonoBehaviour {
	public Dungeon dungeon;
	public TextMesh price;

	public void Inflate() {
		var rand = 80 + Random.Range(0f, 1f);
		price.text = "$" + rand;
	}

	public void SpawnWorld() {
		dungeon.Config.Seed = (uint)Random.Range(0, 1000);
		dungeon.Build();
	}
}
