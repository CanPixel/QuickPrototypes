using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public GameObject TileUI;
	public GameObject body, leg_L, leg_R;
	private Image tileImage;
	public int step;
	private Vector3 baseTileUIScale;

	public Text tileKeyText, tileKeyShadow;
	public GameObject holdTileUI;

	private float move;

	private Vector3 basePos;
	private float UIBaseScale;
	private Rigidbody rb;

	public KeyCode key;
	private bool hit = false;

	private bool keyPressed = false;
	private double pressFrame;

	public double accuracy = 100;

	private bool up = false;
	private float baseLegY;

	private float fin;
	private bool keyHeld = false, sliding = false;

	public static Tile nextTile, currentTile;

	private static Player self;

	void Start () {	
		self = this;
		baseLegY = leg_L.transform.localPosition.y;
		rb = GetComponent<Rigidbody>();
		basePos = transform.position;
		tileImage = TileUI.GetComponent<Image>();
		baseTileUIScale = TileUI.transform.localScale;
	}

	void FixedUpdate() {
		if(nextTile != null) {
			TileUI.transform.position = Vector3.Lerp(TileUI.transform.position, nextTile.transform.position - new Vector3(0.25f, 0, 1), Time.deltaTime * 4);
		
			float scl = Mathf.Sin(Time.time * 4) / 2;
			holdTileUI.transform.localScale = new Vector3(scl, scl, scl) + Vector3.one * 1.5f;

			if(nextTile.keyAction == Tile.KeyAction.HOLD) holdTileUI.SetActive(true);
			else holdTileUI.SetActive(false);
		}
		if(Input.GetKey(key)) keyHeld = true;
		else keyHeld = false;

		if(sliding && !keyHeld) {
			Press_Jump();
			sliding = false;
		}

		float smallBeat = Mathf.Sin((float)AudioSettings.dspTime / BeatGenerator.GetBPM() * 3.5f - 50) / 4f;
		TileUI.transform.localScale = new Vector3(baseTileUIScale.x + smallBeat, baseTileUIScale.y + smallBeat, baseTileUIScale.z);

		tileKeyText.text = tileKeyShadow.text = key.ToString();

		body.transform.localRotation = Quaternion.Euler(Mathf.Sin((float)BeatGenerator.LastBeat() * 4 + 1) / 2.5f, Mathf.LerpAngle(body.transform.localEulerAngles.y, 0, Time.deltaTime * 3), Mathf.LerpAngle(body.transform.localEulerAngles.z, 0, Time.deltaTime * 3));
		leg_L.transform.localRotation = Quaternion.Lerp(leg_L.transform.localRotation, Quaternion.identity, Time.deltaTime * 2);
		leg_R.transform.localRotation = Quaternion.Lerp(leg_R.transform.localRotation, Quaternion.identity, Time.deltaTime * 2);
		if(up) {
			float legL = Mathf.Lerp(leg_L.transform.localPosition.y, baseLegY, Time.deltaTime * 4);
			float legR = Mathf.Lerp(leg_R.transform.localPosition.y, baseLegY * 10, Time.deltaTime * 4);
			leg_L.transform.localPosition = new Vector3(leg_L.transform.localPosition.x, legL, leg_L.transform.localPosition.z);
			leg_R.transform.localPosition = new Vector3(leg_R.transform.localPosition.x, legR, leg_R.transform.localPosition.z);
		} else {
			float legL = Mathf.Lerp(leg_L.transform.localPosition.y, baseLegY * 10, Time.deltaTime * 4);
			float legR = Mathf.Lerp(leg_R.transform.localPosition.y, baseLegY, Time.deltaTime * 4);
			leg_L.transform.localPosition = new Vector3(leg_L.transform.localPosition.x, legL, leg_L.transform.localPosition.z);
			leg_R.transform.localPosition = new Vector3(leg_R.transform.localPosition.x, legR, leg_R.transform.localPosition.z);
		}

		if(NoteHit()) {
			BeatGenerator.TICKHITS++;
			tileImage.color = new Color(0.2f, 0.6f, 0.7f);
			hit = false;
		}
		else tileImage.color = Color.Lerp(tileImage.color, new Color(0.7f, 0.2f, 0.3f), Time.deltaTime * 3);

		if(Input.GetKeyDown(key)) {
			hit = true;
			keyPressed = true;
			BeatGenerator.SetHit();
			pressFrame = AudioSettings.dspTime;
		}
	}

	public void Tick () {
		up = !up;
		if(BeatGenerator.TICK < 4) return;
		leg_R.transform.localRotation = Quaternion.Euler(0, 10 * ((up)? -1 : 1), 0);
		leg_L.transform.localRotation = Quaternion.Euler(0, 10 * ((up)? 1 : -1), 0);
		body.transform.localRotation = Quaternion.Euler(body.transform.localEulerAngles.x, 4, -8);
		keyPressed = false;
		if(NoteHit()) hit = true;
		rb.AddForce(new Vector3(step, 0, 0) * 1000);
		hit = false;
	}

	public double GetLastHitNote() {
		return Mathf.Abs(Mathf.Abs((float)(BeatGenerator.LastBeat()) - Mathf.Abs((float)(pressFrame))) - 0.25f);
	}

	public bool NoteHit() {
		return GetLastHitNote() < accuracy;
	}

	public static void ColorUI() {
		self.tileImage.color = new Color(0.2f, 0.6f, 0.7f);
	}

	public void Press_Jump() {
		rb.velocity = new Vector3(0, step, 0) * 25;
	}

	public void Press_Slide() {
		if(Input.GetKey(key)) sliding = true;
	//	else Debug.Log("Invalid!");
		//if(!keyHeld) Press_Jump();
	}

	public static KeyCode GetKey() {
		return self.key;
	}
}
