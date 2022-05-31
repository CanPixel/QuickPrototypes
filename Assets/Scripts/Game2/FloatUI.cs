using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUI : MonoBehaviour {
	private Vector3 basePos, baseScale;
	
	public float amplitude = 2, speed = 1;
	public Vector3 dir;
	public Vector3 scaleDir;
	public float scaleAmp = 1, scaleSpeed = 1;

	public float initialScale = 1;

	void Start () {
		baseScale = transform.localScale;
		basePos = transform.localPosition;
	}
	
	void FixedUpdate () {
		transform.localPosition = new Vector3(basePos.x * dir.x + Mathf.Sin(Time.time * speed) * amplitude, basePos.y * dir.y + Mathf.Sin(Time.time * speed) * amplitude, basePos.z * dir.z + Mathf.Sin(Time.time * speed) * amplitude);;
		transform.localScale = new Vector3(baseScale.x * scaleDir.x + Mathf.Sin(Time.time * scaleSpeed) * scaleAmp + initialScale, baseScale.y * scaleDir.y + Mathf.Sin(Time.time * scaleSpeed) * scaleAmp + initialScale, baseScale.z * scaleDir.z + Mathf.Sin(Time.time * scaleSpeed) * scaleAmp + initialScale);
	}
}
