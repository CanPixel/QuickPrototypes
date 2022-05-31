using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LOTTERYCODE : MonoBehaviour {
	private InputField input;

	public InputField next;

	void Start () {
		input = GetComponent<InputField>();
	}

	public void KeyChange () {
		if(next == null) return;

		if(input.text.Length >= 0) {
			EventSystem.current.SetSelectedGameObject(next.gameObject, null);
		}
	}
}
