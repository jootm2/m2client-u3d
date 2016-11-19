using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class M2ImagePicker : MonoBehaviour {

	public string LibName;

	public uint ImageIndex;

	// Use this for initialization
	void Awake () {
		string libPath = Path.Combine(Path.Combine(SDK.RootPath, "Data"), LibName);
		RawImage component = gameObject.GetComponent<RawImage> ();
		component.texture = M2Image.M2Image.Image (libPath, ImageIndex);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
