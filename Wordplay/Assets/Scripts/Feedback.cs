using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Feedback : MonoBehaviour {
	private TextMesh mesh;
	private Transform t;
	private Transform cam;
	
	private Color[] colours = new Color[] { new Color (0.3f, 0, 0.6f, 1f)/*purplish*/, new Color (0.3f, 0.6f, 0.9f, 1f), new Color(0.05f, 0.05f, 0.4f)};
	public String[] messages = new String[] { "No, that's not it! R to restart", "Ah! Then what?", "Let's see... Z to begin", "What really happened?"};
	
	public static Feedback instance;
	public bool followCamera = false;
	private float displayTime = 2f;
	void Awake () {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		mesh = GetComponent<TextMesh>();
		mesh.renderer.material = Resources.Load("Textymat") as Material;
		t = transform;
		cam = WordCamera.instance.transform;
		Neutral();
	}
	
	// Update is called once per frame
	void Update () {
		if (followCamera){
			t.position = new Vector3(cam.position.x, t.position.y, t.position.z);
		}
	}
	
	public void Neutral () {
		mesh.text = messages[2];
		mesh.renderer.material.color = colours[2];
	}
	
	public void Restart () {
		mesh.text = messages[3];
		mesh.renderer.material.color = colours[2];
	}
	
	public void Disapprove () {
		mesh.text = messages[0];
		mesh.renderer.material.color = colours[0];
		Invoke("Restart", displayTime);
	}
	
	public void Approve () {
		mesh.text = messages[1];
		mesh.renderer.material.color = colours[1];
	}
}
