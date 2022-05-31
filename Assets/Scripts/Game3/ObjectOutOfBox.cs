using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectOutOfBox : MonoBehaviour {
	public GameObject target;
	public UnityEvent Event;

	private bool finished = false;

	public bool OnEnter = false, AfterDelay = false;
	private float enterDelay = 0;

	void OnTriggerStay(Collider col) {
		if(OnEnter && col.gameObject == target) {
			if(enterDelay > 0) enterDelay += Time.deltaTime;
			if(enterDelay > 2.1f) {
				Event.Invoke(); 
				enterDelay = 0;	
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.gameObject == target) {
			if(!AfterDelay && !finished) {
				Event.Invoke();
				finished = true;
			}
			else {
				if(!finished) enterDelay = 0;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if(OnEnter && col.gameObject == target && !finished) {
			if(!AfterDelay) Event.Invoke();
			enterDelay = 0.1f;
			finished = true;
		}
	}
}
