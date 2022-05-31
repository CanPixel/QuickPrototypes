using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bump : MonoBehaviour {
	public bool scale = true, pos = false;

	public Vector3 bumpAmplitude;
	public Vector3 bumpSpeed; 

	private Vector3 basePos, baseScale;

	void Start () {
		basePos = transform.localPosition;
		baseScale = transform.localScale;
	}
	
	void FixedUpdate () {
		Vector3 lerpPos = new Vector3(Mathf.Sin(Time.time * bumpSpeed.x) * bumpAmplitude.x, Mathf.Cos(Time.time * bumpSpeed.y) * bumpAmplitude.y, Mathf.Sin(Time.time * bumpSpeed.z) * bumpAmplitude.z);
		if(pos) transform.localPosition = basePos + lerpPos;
	
		Vector3 lerpScale = new Vector3(Mathf.Sin(Time.time * bumpSpeed.x) * bumpAmplitude.x, Mathf.Cos(Time.time * bumpSpeed.y) * bumpAmplitude.y, 1);
		if(scale) transform.localScale = baseScale + lerpScale;
	}
}
