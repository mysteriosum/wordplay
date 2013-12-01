using UnityEngine;
using System.Collections;

public class Timidity : MonoBehaviour {
	
	public Transform node1;
	public Transform node2;
	
	private Transform t;
	private Transform p;
	private float lerpAmount = 0.4f;
	
	// Use this for initialization
	void Start () {
		if (!node1 || !node2){
			Debug.LogWarning("There's not enough nodes on this timid script");
		}
		t = transform;
		p = Quill.player.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		Transform targetNode;
		float dist1 = (node1.position - p.position).magnitude;
		float dist2 = (node2.position - p.position).magnitude;
		
		targetNode = dist1 > dist2? node1 : node2;
		RaycastHit hitInfo;
		Ray ray = new Ray(t.position, targetNode.position - t.position);
		
		bool hit = Physics.Raycast(ray, out hitInfo);		//shoot a ray to detect the avatar
		if (hit){
			if (hitInfo.collider.name == p.name)
				targetNode = targetNode == node2? node1 : node2;	//change nodes if the avatar's in between us.
			Debug.Log("Hit!");
		}
		
		t.position = Vector3.Lerp(t.position, targetNode.position, lerpAmount);
	}
}
