using UnityEngine;
using System.Collections;

public class M2SoundPicker : MonoBehaviour {

	/// <summary>
	/// 声音索引
	/// </summary>
	public int soundIndex;
	/// <summary>
	/// 是否在加载后立即播放
	/// </summary>
	public bool playImmediately = true;
	/// <summary>
	/// 是否是客户端程序使用的音频(不在lst里)
	/// </summary>
	public bool offLst = false;

	void Awake() {
		AudioSource _as = gameObject.GetComponent<AudioSource> ();
		_as.clip = M2Sound.Get(soundIndex, offLst);
		if (playImmediately)
			_as.Play ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}