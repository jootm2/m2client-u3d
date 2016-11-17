using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UsernameValidator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InputField input = gameObject.GetComponent<InputField> ();
		input.onValidateInput = _OnValidateInput;
	}

	readonly char[] unachars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890_@".ToCharArray ();
	char _OnValidateInput(string text, int charIndex, char addedChar) {
		for (int i = 0; i < unachars.Length; ++i)
			if (unachars [i] == addedChar)
				return addedChar;
		return '\0'; // 返回空
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
