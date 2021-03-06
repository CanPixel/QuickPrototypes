using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloat : MonoBehaviour {
	private Vector3 targetPos, targetScale, targetRot;
	private Vector3 basePos, baseScale, baseRot;
	public float scaleSpeed = 0.5f, translateSpeed = 0.5f, rotateSpeed = 0.5f;

	public Vector3 posDir = new Vector3(0, 1, 0), scaleDir = new Vector3(0, 0, 0), rotDir = new Vector3(0, 0, 0);
	public float posFreq = 0, scaleFreq = 0;
	public float posAmp = 1, scaleAmp = 1;
	public float rotFreq = 0, rotAmp = 1;

	public Vector3 scaleOffset = new Vector3(0, 0, 0);
	public float posFreqOffs = 0;

	void Start () {
		basePos = transform.localPosition;
		baseScale = transform.localScale;
		baseRot = transform.localEulerAngles;
	}
	
	void Update () {
		targetPos = basePos + posDir * Mathf.Sin(Time.time * posFreq + posFreqOffs) * posAmp;
		targetScale = baseScale + scaleDir * Mathf.Cos(Time.time * scaleFreq) * scaleAmp;
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * translateSpeed);
		transform.localScale = Vector3.Lerp(transform.localScale, targetScale + scaleOffset, Time.deltaTime * scaleSpeed);

		if(rotDir != Vector3.zero) {
			targetRot = baseRot + rotDir * Mathf.Sin(Time.time * rotFreq) * rotAmp;
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * rotateSpeed);
		}
	}

	public void SetBaseScale(Vector3 sc) {
		baseScale = sc;
	}

	public void UpdatePositions(Vector3 pos) {
		basePos = pos;
	}
}
