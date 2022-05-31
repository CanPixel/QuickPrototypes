using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAfter : MonoBehaviour {
	public float after = 2;
	private float time = 0;

	public UnityEvent postEvent;
	public bool once = true;

	private bool ran = false;

	void FixedUpdate () {
		time += Time.deltaTime;
		if(time > after) {
			if(once && !ran) {
				postEvent.Invoke();
				ran = true;
			} else if(!once) postEvent.Invoke();
		}
	}
}
