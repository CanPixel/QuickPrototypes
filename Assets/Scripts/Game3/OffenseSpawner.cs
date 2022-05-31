using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OffenseSpawner : MonoBehaviour {
	public GameObject offensePrefab;
	public Transform UI;

	public GameObject scoreUI, missionUI, startUI, title, endUI;

	public Text score, scoreShadow;
	public Text time, timeShadow;
	public int offense = 0;

	private static OffenseSpawner self;
	private float cutsceneTick = 0;

	private bool play = false;
	private float gameTime = 0;

	private int scoreSecs, scoreMins;

	private bool ended = false;

	void Awake() {
		self = this;
		scoreUI.SetActive(false);
		missionUI.SetActive(false);
		startUI.SetActive(false);
		endUI.SetActive(false);
	}

	void FixedUpdate() {
		if(ended) {
			if(Input.GetKey(KeyCode.F5)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		
		if(play) {
			gameTime += Time.deltaTime;
			int seconds = (int)gameTime;
			int minutes = (int)(gameTime / 60f);
			seconds -= (60 * minutes);
			scoreSecs = seconds;
			scoreMins = minutes;
		}

		time.text = timeShadow.text = scoreMins + ":" + scoreSecs;

		if(title == null) {
			if(cutsceneTick < 1) {
				startUI.SetActive(true);
				User.Bumpy(true);
			}
			cutsceneTick += Time.deltaTime;
		}
		if(cutsceneTick > 1.5f && cutsceneTick <= 2.5f) {
			User.Bumpy(false);
			User.Rage();
			missionUI.SetActive(true);
			startUI.SetActive(false);
		}
		if(cutsceneTick > 4f && missionUI.activeSelf) missionUI.SetActive(false);
		if(cutsceneTick > 4.5f && !scoreUI.activeSelf) BeginGame();
	}

	protected void BeginGame() {
		scoreUI.SetActive(true);
		User.ReadyPlayer();
		play = true;
	}

	public static void EndGame() {
		self.play = false;
		self.endUI.SetActive(true);
		self.ended = true;
	}

	public static void Offend(string offense) {
		self.SpawnOffense(offense);
	}

	public void SpawnOffense(string offense) {
		GameObject obj = Instantiate(offensePrefab, new Vector3(1000, 200, 0), Quaternion.identity);
		obj.GetComponent<CriminalOffense>().SetOffense(offense.ToUpper());
		obj.transform.SetParent(UI);
		this.offense++;
		score.text = scoreShadow.text = this.offense.ToString();
	}
}
