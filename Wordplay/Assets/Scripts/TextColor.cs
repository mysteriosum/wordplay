using UnityEngine;
using System.Collections;

public class TextColor : MonoBehaviour {
	TextMesh mesh;
	public Color colour;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
		if (mesh){
			mesh.renderer.material.color = colour;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
