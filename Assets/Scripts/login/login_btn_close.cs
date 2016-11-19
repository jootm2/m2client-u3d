using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class login_btn_close : MonoBehaviour, IPointerUpHandler {
	
	public void OnPointerUp (PointerEventData eventData) {
		RectTransform rect = gameObject.GetComponent<RectTransform> ();
		if (RectTransformUtility.RectangleContainsScreenPoint (gameObject.GetComponent<RectTransform> (), eventData.position)) {
			Application.Quit ();
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
