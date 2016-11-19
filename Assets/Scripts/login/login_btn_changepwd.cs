using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class login_btn_changepwd : MonoBehaviour, IPointerUpHandler {

	public void OnPointerUp (PointerEventData eventData) {
		RectTransform rect = gameObject.GetComponent<RectTransform> ();
		if (RectTransformUtility.RectangleContainsScreenPoint (gameObject.GetComponent<RectTransform> (), eventData.position)) {
			GameObject.Find ("canvas_login_main").GetComponent<Canvas> ().enabled = false;
			GameObject.Find ("canvas_changepwd_main").GetComponent<Canvas> ().enabled = true;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}