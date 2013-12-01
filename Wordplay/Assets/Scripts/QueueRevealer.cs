using UnityEngine;
using System.Collections;

public class QueueRevealer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("My render queue is " + renderer.material.shader.renderQueue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
