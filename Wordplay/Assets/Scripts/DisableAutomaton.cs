using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DisableAutomaton : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		
		Automaton a = other.GetComponent<Automaton>();
		
		if (a != null){
			a.BroadcastMessage("Deactivate", SendMessageOptions.DontRequireReceiver);
		}
	}
}
