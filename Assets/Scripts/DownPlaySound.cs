using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DownPlaySound : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown (PointerEventData eventData) {
		gameObject.GetComponent<AudioSource> ().Play ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
