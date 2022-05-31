using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
	public static Level self;

	public GameObject player;
	private Player playerScr;

	public GameObject tile, slide;

	public float levelY = -17;
	public float scaling = 100;
	public int worldSize = 100;

	protected List<Tile> tiles = new List<Tile>();

	void Awake() {
		self = this;
		playerScr = player.GetComponent<Player>();

		Generate();
	}

	protected void Generate() {
		int slide = 0;
		bool previousSlide = false;
		for(int i = 0; i < worldSize; i++) {
			bool condition = GenerateUp(i);
			if(Random.Range(0, 13) < 2 && slide <= 0) slide = 1;
			
			if(slide <= 0) {
				if(previousSlide) condition = false;
				GenerateTile(i * scaling, levelY + (condition ? scaling : 0), condition);
				previousSlide = false;
			}
			else {
				GenerateSlide(i * scaling, levelY);
				previousSlide = true;
				slide++;
			}
			if(slide > 4) slide = 0;
			if(slide <= 0 && condition && Random.Range(0, 5) < 2) levelY += scaling;
		}

		for(int i = 0; i < tiles.Count; i++) {
			if(i > 0) tiles[i].previous = tiles[i - 1];
			if(i < tiles.Count - 1) tiles[i].next = tiles[i + 1];
		}

		for(int i = 0; i < tiles.Count; i++) {
			if(i > 0 && tiles[i].next != null && tiles[i].next.up) tiles[i].onTile += playerScr.Press_Jump;
		}
	}

	protected bool GenerateUp(int i) {
		return (Random.Range(0, 50) < 10) && i > 5;
	}

	protected void GenerateSlide(float x, float y) {
		GameObject slides = Instantiate(slide, new Vector2(x, y), Quaternion.Euler(-90, 0, 0));
		slides.transform.SetParent(transform);
		slides.GetComponent<Tile>().onTile += playerScr.Press_Slide;
	}

	protected void GenerateTile(float x, float y, bool up = false) {
		GameObject tiles = Instantiate(tile, new Vector2(x, y), Quaternion.Euler(-90, 0, 0));
		tiles.transform.SetParent(transform);
		if(up) tiles.GetComponent<Tile>().up = up;
		this.tiles.Add(tiles.GetComponent<Tile>());
	}
	
	public static void Tick() {
		Tile.tiled = false;
		self.playerScr.Tick();
	}

	public static Player GetPlayer() {
		return self.playerScr;
	}
}
