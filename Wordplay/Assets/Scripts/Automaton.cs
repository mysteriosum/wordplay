using UnityEngine;
using System.Collections;

public class Automaton : Movable {
	
	[System.SerializableAttribute]
	public class Automation {
		public float hDistanceToTravel;
		public float waitTime;
		public float jumpTiming;
		public bool goingRight = true;
		
		private float distTraveled;
		public float DistTraveled {
			get { return distTraveled; }
			set {
				distTraveled = value;
			}
		}
		
		private bool waiting = false;
		public bool Waiting {
			get { return waiting; }
			set { waiting = value; }
		}
		
		private float waitTimer = 0;
		public float WaitTimer {
			get { return waitTimer; }
			set { waitTimer = value; }
		}
		
		private float jumpTimer = 0;
		public float JumpTimer {
			get { return jumpTimer; }
			set { jumpTimer = value; }
		}
	}
	
	private bool active = true;
	
	public Automation auto;
	
	// Use this for initialization
	void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if (!active)
			return;
		
		base.Update();
		
		if (grounded){
			if (auto.Waiting){
				velocity = Move(velocity, 0);
				auto.WaitTimer += Time.deltaTime;
				if (auto.WaitTimer >= auto.waitTime){
					auto.Waiting = false;
					auto.DistTraveled = 0;
					auto.WaitTimer = 0;
				}
			}
			else {
				velocity = Move(velocity, auto.goingRight? 1 : -1);
				auto.DistTraveled += Mathf.Abs(velocity.x * Time.deltaTime);
				if (auto.DistTraveled >= auto.hDistanceToTravel){
					auto.Waiting = true;
					auto.WaitTimer = 0;
					auto.goingRight = auto.goingRight? false : true;
				}
			}
			
			auto.JumpTimer += Time.deltaTime;
			if (auto.JumpTimer >= auto.jumpTiming){
				Jump(initialJumpVelocity);
				auto.JumpTimer = 0;
			}
		}
		else {
			velocity = Move(velocity, 0);
		}
		
	}
	
	void LateUpdate () {
		base.LateUpdate();
	}
	
	public void Deactivate() {
		velocity = Vector2.zero;
		TextCollection.Instance.reset += Restart;
		active = false;
	}
	
	public override void Restart() {
		base.Restart();
		active = true;
		BroadcastMessage("Activate", SendMessageOptions.DontRequireReceiver);
	}
}
