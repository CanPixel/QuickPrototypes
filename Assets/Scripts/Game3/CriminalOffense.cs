using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CriminalOffense : MonoBehaviour {
	public float delay = 7;

	private float show = 0;

	private string offense = ""; 

	public Text offenseUI;

	void FixedUpdate () {
		show += Time.deltaTime;
	
		offenseUI.text = offense;
		if(show < delay) transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * 4);
		else transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-2000, 0, 0), Time.deltaTime * 5);

		if(show > delay*2) Destroy(gameObject);
	}

	public void SetOffense(string off) {
		offense = off;
	}
}
