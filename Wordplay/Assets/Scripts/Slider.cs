using UnityEngine;
using System.Collections;

public class Slider : MonoBehaviour {
	
	public Transform[] nodes;
	public float moveSpeedPerSec = 1.5f;
	public float waitBetweenNodes = 1f;
	
	private float waitTimer = 0;
	private bool waiting = false;
	
	private Transform t;
	private int curNode = 0;
	
	private float distTraveled;
	private float[] distToTravel;
	
	private int NextNode {
		get {
			if (curNode < nodes.Length - 1){
				return curNode + 1;
			}
			else{
				return 0;
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		t = transform;
		
		if (nodes.Length <= 1){
			Debug.LogWarning("There's not enough nodes on this word: " + name);
			Destroy(gameObject);
			return;
		}
		distToTravel = new float[nodes.Length];
		for (int i = 0; i < nodes.Length; i ++){
			distToTravel[i] = (nodes[i].position - nodes[i == nodes.Length - 1? 0 : i + 1].position).magnitude;
		}
		
		//t.position = nodes[curNode].position;
	}
	
	// Update is called once per frame
	void Update () {
		if (waiting){
			waitTimer += Time.deltaTime;
			if (waitTimer > waitBetweenNodes){
				waiting = false;
				waitTimer = 0;
			}
		}
		else {
			Vector3 amount = (nodes[NextNode].position - nodes[curNode].position).normalized * moveSpeedPerSec * Time.deltaTime;
			t.Translate(amount, Space.World);
			distTraveled += amount.magnitude;
			if (distTraveled >= distToTravel[curNode]){
				waiting = true;
				curNode = NextNode;
				distTraveled = 0;
			}
		}
	}
}
