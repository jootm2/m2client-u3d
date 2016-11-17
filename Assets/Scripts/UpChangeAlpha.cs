using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpChangeAlpha : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public void OnPointerDown (PointerEventData eventData) {
		RawImage img = gameObject.GetComponent<RawImage>();
		img.color = Color.white;
	}

	public void OnPointerUp (PointerEventData eventData) {
		RawImage img = gameObject.GetComponent<RawImage>();
		img.color = new Color(0, 0, 0, 0);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
