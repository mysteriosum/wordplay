using UnityEngine;
using System.Collections;

public class FireBreather : MonoBehaviour {
	
	private bool breathing = true;
	private float breathTimer = 0;
	public float breathTiming = 2f;
	private UnityEngine.Object breath;
	
	private Transform t;

	// Use this for initialization
	void Start () {
		breath = Resources.Load("pre_fireBreath");
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (!breathing) return;
		
		breathTimer += Time.deltaTime;
		if (breathTimer >= breathTiming){
			Instantiate(breath, t.position, t.rotation);
			breathTimer = 0;
		}
	}
	
	
	void Deactivate () {
		breathing = false;
	}
	
	void Activate () {
		breathing = true;
	}
}
