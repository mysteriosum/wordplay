using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AxeTrigger : MonoBehaviour {
	
	public Transform bridge;
	private List<Transform> pieces = new List<Transform>();
	private Transform t;
	private float destroyInterval = 0.115f;
	private float destroyTimer = 0;
	private bool collapsing = false;
	private int justDestroyed = 0;

	// Use this for initialization
	void Start () {
		t = transform;
		if (bridge.childCount == 0){
			Debug.LogWarning ("Your bridge has no pieces!");
			Destroy(this);
		}
		int counter = 0;
		while (pieces.Count < bridge.childCount){
			Transform closestT = null;
			float closest = float.MaxValue;
			int index;
			for (index = 0; index < bridge.childCount; index++){
				Transform piece = bridge.GetChild(index);
				if (pieces.Contains(piece))
					continue;
				
				float diff = t.position.x - piece.position.x;
				if (diff < closest){
					closest = diff;
					closestT = piece;
				}
			}
			if (closestT != null)
				pieces.Add(closestT);
			counter++;
			if (counter > 100){
				Debug.LogWarning("Either you have a long ass bridge and need to change this code, or you fucked up");
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (collapsing && justDestroyed < pieces.Count){
			destroyTimer += Time.deltaTime;
			if (destroyTimer > justDestroyed * destroyInterval){
				pieces[justDestroyed].gameObject.SetActive(false);
				justDestroyed ++;
			}
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			collapsing = true;
			TextCollection.Instance.reset += Reset;
		}
	}
	
	void Reset () {
		foreach (Transform t in pieces){
			t.gameObject.SetActive(true);
		}
		justDestroyed = 0;
		destroyTimer = 0;
		collapsing = false;
	}
}
