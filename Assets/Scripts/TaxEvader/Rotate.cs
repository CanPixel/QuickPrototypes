using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	public Vector3 dir;

	void FixedUpdate () {
		var fin = dir * Time.deltaTime;

		transform.Rotate(fin.x, fin.y, fin.z);
	}
}
