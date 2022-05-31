using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoTo : MonoBehaviour {
	public float after = 2;
	public Vector3 destination;
	public float speed = 1;

	private float time = 0;

	void FixedUpdate () {
		time += Time.deltaTime;
		if(time > after) {
			if(GetComponent<FloatUI>() != null) GetComponent<FloatUI>().enabled = false;
			transform.localPosition = Vector3.Lerp(transform.localPosition, destination, Time.deltaTime * speed);
		}
	}
}
