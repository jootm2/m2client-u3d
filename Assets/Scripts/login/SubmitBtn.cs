using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitBtn : MonoBehaviour, IPointerUpHandler {

	public void OnPointerUp (PointerEventData eventData) {RectTransform rect = gameObject.GetComponent<RectTransform> ();
		if (RectTransformUtility.RectangleContainsScreenPoint (gameObject.GetComponent<RectTransform> (), eventData.position)) {
			string una = GameObject.Find ("TxtUsername").GetComponent<InputField> ().text;
			string pwd = GameObject.Find ("TxtPassword").GetComponent<InputField> ().text;
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
