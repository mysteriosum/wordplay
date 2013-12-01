using UnityEngine;
using System.Collections;

public class FireBreath : Movable {
	
	private int direction = -1; //always goes left! (until further notice)
	
	private float destroyTimer = 2.5f;
	
	// Use this for initialization
	void Start () {
		base.Start();
		Destroy(gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
		velocity = Move(velocity, direction);
		
		base.Update();
	}
	
}
