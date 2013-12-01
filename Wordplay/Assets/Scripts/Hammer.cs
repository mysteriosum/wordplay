using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Hammer : Movable {
	
	private int direction = -1; //always goes left! (until further notice)
	private TextCollectible texto;
	
	private int rotateTimer = 0;
	private int rotateTiming = 6;
	private int rotateAmount = 45;
	
	// Use this for initialization
	void Start () {
		base.Start();
		texto = GetComponent<TextCollectible>();
		translateBy = Space.World;
		Jump(initialJumpVelocity);
	}
	
	// Update is called once per frame
	void Update () {
		velocity = Move(velocity, direction);
		
		rotateTimer ++;
		if (rotateTimer >= rotateTiming){
			t.Rotate(new Vector3(0, 0, rotateAmount));
			rotateTimer = 0;
		}
		
		base.Update();
	}
	
	void OnTriggerEnter (Collider other){
		if (other.collider.gameObject.layer == LayerMask.NameToLayer("Collisions")){
			Destroy(gameObject);
		}
	}
	
}
