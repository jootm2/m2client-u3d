using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class login_btn_submit : MonoBehaviour, IPointerUpHandler {

	public void OnPointerUp (PointerEventData eventData) {RectTransform rect = gameObject.GetComponent<RectTransform> ();
		if (RectTransformUtility.RectangleContainsScreenPoint (gameObject.GetComponent<RectTransform> (), eventData.position)) {
			string una = GameObject.Find ("input_login_una").GetComponent<InputField> ().text;
			string pwd = GameObject.Find ("input_login_pwd").GetComponent<InputField> ().text;
			KBEngine.Event.fireIn ("login", una, pwd);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
