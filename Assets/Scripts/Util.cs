using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

	public static Component CopyComponent(Component original, GameObject dest) {
		System.Type type = original.GetType();
		Component copy = dest.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach(System.Reflection.FieldInfo field in fields) field.SetValue(copy, field.GetValue(original));
		return copy;
	}

}
