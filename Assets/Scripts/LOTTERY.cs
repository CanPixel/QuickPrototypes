using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOTTERY : MonoBehaviour {
	public GameObject kader, logo, kader2, kader3, kader4;

	private bool move = false;
	private Vector3 scale;

	private float wait = 1;
	private float time = 0;

	void Update() {
		if(Input.GetKeyDown(KeyCode.A)) {
			kader3.SetActive(true);
			kader4.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.B)) {
			kader3.SetActive(false);
			kader4.SetActive(true);
		}
	}

	void FixedUpdate() {
		if(move) {
			time += Time.deltaTime;

			kader.transform.localPosition = Vector3.Lerp(kader.transform.localPosition, new Vector3(0, 335, 0), Time.deltaTime * 1f);
			logo.transform.localPosition = Vector3.Lerp(logo.transform.localPosition, new Vector3(-350, -100, 0), Time.deltaTime * 2f);

			kader2.transform.position += Vector3.down * Time.deltaTime * 800f;
			if(time > wait) kader3.transform.localScale = Vector3.Lerp(kader3.transform.localScale, scale, Time.deltaTime * 4f);
		}
	}

	public void CalcLottery() {
		move = true;
		scale = kader3.transform.localScale;
		kader3.transform.localScale = Vector3.zero;
		kader3.SetActive(true);

		kader.GetComponent<UIFloat>().UpdatePositions(new Vector3(0, 335, 0));
		logo.GetComponent<UIFloat>().UpdatePositions(new Vector3(-350, -100, 0));
	}
}
