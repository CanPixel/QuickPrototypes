using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlatform : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private float heldTick = 0;

	private bool hover = false;
	private SpriteRenderer sprite;
	private Image image;

	private RectTransform rect;

	private bool grabbed = false, released = false, rotate = false;

	void Awake() {
		rect = GetComponent<RectTransform>();
		sprite = GetComponent<SpriteRenderer>();
		image = GetComponent<Image>();
		if(sprite != null && image != null) sprite.enabled = false;
	}

	void FixedUpdate() {
		if(hover && !released && Input.GetMouseButton(0) && sprite != null) {
			heldTick += Time.deltaTime;
			transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 30) * 4f);
			if(heldTick > 0.2f) Grab();
		} 
		if(rotate) transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 30) * 4f);
		else transform.localRotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.localEulerAngles.z, 0, Time.deltaTime * 8));
	
		if(grabbed && !Input.GetMouseButton(0)) Release();
	}

	protected void Release() {
		sprite.enabled = true;
		image.enabled = false;
		GameObject obj = new GameObject("UI Platform");
		obj.tag = "Platform";
		SpriteRenderer spr = obj.AddComponent<SpriteRenderer>();
		spr.sprite = sprite.sprite;
		Util.CopyComponent(GetComponent<Collider>(), obj);
		Vector3 point = Input.mousePosition;
		point.z = 25;
		point = Camera.main.ScreenToWorldPoint(point);

		obj.transform.position = point;
		grabbed = false;
		released = true;
		CustomCursor.lockColor = false;
		Destroy(gameObject);
	}

	protected void Grab() {
		GetComponent<FloatUI>().enabled = false;
		transform.SetParent(CustomCursor.GetCursor().transform);
		transform.localPosition = Vector3.zero;
		rect.position = Vector3.zero;
		rect.anchoredPosition  = rect.pivot = new Vector2(0, 0);
		rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
		grabbed = true;
		CustomCursor.lockColor = true;
	}

	public void OnPointerEnter(PointerEventData d) {
		CustomCursor.SetCursor(Color.black);
		hover = true;
		rotate = true;
	}

	public void OnPointerExit(PointerEventData d) {
		CustomCursor.SetCursor(Color.white);
		hover = false;
		heldTick = 0;
		rotate = false;
	}
}
