using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HammerThrow : MonoBehaviour {
	
	public float throwInterval = 1.5f;
	public int amount = 1;
	
	private int waitFrames = 6;
	private float throwTimer = 0f;
	private int hammerCount = 0;
	private int justThrew = 0;
	private UnityEngine.Object hammer;
	private Transform t;
	private bool throwing = true;
	
	public String[] hammerString = new String[] { "h", "a", "m", "m", "e", "r"};
	private int currentLetter = 0;
	
	private int NextLetter {
		get { return currentLetter == hammerString.Length - 1? 0 : currentLetter + 1; }
	}
	
	// Use this for initialization
	void Start () {
		t = transform;
		hammer = Resources.Load("pre_hammer");
	}
	
	// Update is called once per frame
	void Update () {
		if (!throwing)
			return;
		
		throwTimer += Time.deltaTime;
		
		if (throwTimer >= throwInterval){
			GameObject newHammer = Instantiate(hammer, t.position, t.rotation) as GameObject;
			
			justThrew ++;
			if (justThrew >= amount){
				throwTimer = 0;
				justThrew = 0;
			}
			else{
				throwTimer = throwInterval - (waitFrames * Time.deltaTime);
			}
			
			
			newHammer.SendMessage("SetText", hammerString[currentLetter], SendMessageOptions.RequireReceiver);
			currentLetter = NextLetter;
		}
	}
	
	void Deactivate(){
		throwing = false;
	}
	
	void Activate(){
		throwing = true;
	}
}
