using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIPlatformer;

public class TrackGenerator : MonoBehaviour {
	public GameObject[] obstacles;

	[SerializeField]
	private int ticks = 0;

	public Vector2 bounds = new Vector2(-6, 13);
	public Vector2 scaleBoundsX = new Vector2(1.5f, 10), scaleBoundsY = new Vector2(2, 8);

	private Vector3 lastPos, lastScale;
	
	void Start () {
		StartCoroutine(Generate());
	}

	private IEnumerator Generate() {
		while(true) {
			if(ticks > 1 && FlatPlayer.IsMoving()) GenPlatform();
			ticks++;
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	private void GenPlatform() {
		GameObject obst = obstacles[Random.Range(0, obstacles.Length)];
		GameObject plat = Instantiate(obst, Vector3.zero, Quaternion.identity);
		plat.name = obst.name;
		plat.transform.SetParent(transform);
		plat.transform.localPosition = new Vector3(FlatPlayer.XPOS + 20 + Random.Range(0, 4) + lastScale.x, Random.Range(bounds.x, bounds.y) - 0.3f + Random.Range(-4, 7), 0);
		plat.transform.localScale = new Vector3(Random.Range(scaleBoundsX.x, scaleBoundsX.y), Random.Range(scaleBoundsY.x, scaleBoundsY.y), 1);
		lastPos = plat.transform.localPosition;
		lastScale = plat.transform.localScale;
	}
}
