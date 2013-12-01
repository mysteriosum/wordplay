using UnityEngine;
using System.Collections;

public class WordCamera : MonoBehaviour {
	
	private Transform t;
	public Transform[] nodes;
	
	public static WordCamera instance;
	
	private bool moving = false;
	private bool started = false;
	private float moveAmount = 0.075f;
	
	private string levelName;
	
	
	// Use this for initialization
	void Awake () {
		instance = this;
		
	}
	
	void Start () {
		Application.targetFrameRate = 30;
		if (nodes.Length == 0){
			nodes = GameObject.FindSceneObjectsOfType(typeof(GizmoDad)) as Transform[];
		}
		t = transform;
		t.position = nodes[0].position;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving){
			t.position = Vector3.Lerp(t.position, nodes[0].position, moveAmount);
			if ((nodes[0].position - t.position).magnitude < moveAmount){
				t.position = nodes[0].position;
				Application.LoadLevel(levelName);
			}
		}
		else if (started){
			t.position = Vector3.Lerp(t.position, nodes[1].position, moveAmount);
			
		}
		
		if (!started){
			started = Input.GetKey(KeyCode.Z);
		}
	}
	
	public void End (string levelName) {
		this.levelName = levelName;
		moving = true;
	}
}
