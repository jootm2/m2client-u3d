using UnityEngine;
using System.Collections;

public class SDK {
	public static string RootPath { get; private set; }
	static SDK(){
		RootPath = Application.dataPath;
	}
}