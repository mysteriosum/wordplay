using UnityEngine;
using System.Collections;

public class AfterImage : MonoBehaviour {
	private float fadeRate = 0.2f;
	private float startAlpha = 0.05f;
	
	private Material mat;
	private Color target;
	
	
	// Use this for initialization
	void Start () {
		mat = renderer.material;
		target = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
		mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, startAlpha);
		
		Destroy(gameObject, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		mat.color = Vector4.Lerp(mat.color, target, fadeRate);
		
		
	}
}
