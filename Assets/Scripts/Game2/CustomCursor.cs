using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour {
	public GameObject cursorIMG;
	private Image cursor;

	private Color targetCol = Color.white;
	private static CustomCursor self;

	public static bool lockColor = false;
	
	void Awake () {
		self = this;
		Cursor.visible = false;
		cursor = cursorIMG.GetComponent<Image>();
	}

	 void FixedUpdate() {
		cursorIMG.transform.position = Input.mousePosition;

		cursor.color = Color.Lerp(cursor.color, targetCol, Time.deltaTime * 8);

		float scal = Mathf.Sin(Time.time * 9) * 0.15f + 0.6f;
		cursorIMG.transform.localScale = new Vector3(scal, scal, scal);
	 } 

	 public static void SetCursor(Color col) {
		 if(lockColor) return;
		 self.targetCol = col;
	 }

	 public static GameObject GetCursor() {
		 return self.cursorIMG;
	 }

	 public static Transform GetWorld() {
		 return self.transform;
	 }
}
