using UnityEngine;
using System.Collections;

public class Quill : Movable {
	
	
	//LOTS OF DELEGATES! YEAAAAAAAAAAAAAAAAAAAAAAAH
	public delegate void InputDelegate();
	public delegate void FloatyDelegate (float para);
	
	public InputDelegate run;
	public InputDelegate runUp;
	public InputDelegate runDown;
	
	public InputDelegate jump;
	public InputDelegate jumpUp;
	public InputDelegate jumpDown;
	
	public InputDelegate doubleTap;
	public InputDelegate aboutFace;
	
	public InputDelegate downDown;
	public InputDelegate downUp;
	
	public InputDelegate directionDown;
	public FloatyDelegate direction;
	public InputDelegate directionUp;
	
	public InputDelegate fall;
	public InputDelegate gravity;
	
	public static Quill player;
	private bool started = false;
	
	private float maxPress2Jump = 0.25f;
	private float jumpTimer = 0;
	private float jumpInitHeight = Mathf.NegativeInfinity;
	private float multiJumpWindow = 0.74f;
	
	private Material leftArrow;
	private Material rightArrow;
	private Material jumpIndicator;
	private Color activeColour = Color.white;
	private Color inactiveColour = new Color(1f, 1f, 1f, 0.3f);
	private float indicatorLerpage = 0.1f;
	// Use this for initialization
	
	void Awake () {
		base.Awake();
		player = this;
	}
	
	void Start () {
		base.Start();
		
		for (int i = 0; i < t.childCount; i++){
			Transform child = t.GetChild(i);
			if (child.name == "arrowLeft"){
				leftArrow = child.renderer.material;
				continue;
			}
			if (child.name == "arrowRight"){
				rightArrow = child.renderer.material;
				continue;
			}
			if (child.name == "jumpIndicator"){
				jumpIndicator = child.renderer.material;
				continue;
			}
		}
		
		if (rightArrow == null || leftArrow == null || jumpIndicator == null){
			Debug.LogWarning("Quill is missing some children");
		}
		else{
			leftArrow.color = inactiveColour;
			rightArrow.color = inactiveColour;
			jumpIndicator.color = inactiveColour;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!started){
			if (Input.GetButton("Jump"))
				started = true;
			return;
		}
		
		if (Input.GetButton("Jump")){
			if (jumpTimer == float.MaxValue)
				jumpTimer = 0;
			jumpTimer += Time.deltaTime;
			jumpIndicator.color = Color.Lerp(jumpIndicator.color, activeColour, indicatorLerpage);
		}
		else{
			jumpTimer = float.MaxValue;
			jumpIndicator.color = Color.Lerp(jumpIndicator.color, inactiveColour, indicatorLerpage);
		}
		
		//input: I'll get -1, 1 or 0 depending on what's pressed (arrows only - must expand)
		float input = (Input.GetKey(KeyCode.LeftArrow)? -1 : 0) + (Input.GetKey(KeyCode.RightArrow)? 1 : 0);
		if (input > 0){
			rightArrow.color = Color.Lerp(rightArrow.color, activeColour, indicatorLerpage);		//set my arrows to the correct alpha transperency settings
			leftArrow.color = Color.Lerp(leftArrow.color, inactiveColour, indicatorLerpage);
		}
		else if (input < 0){
			rightArrow.color = Color.Lerp(rightArrow.color, inactiveColour, indicatorLerpage);
			leftArrow.color = Color.Lerp(leftArrow.color, activeColour, indicatorLerpage);
		}
		else {
			rightArrow.color = Color.Lerp(rightArrow.color, inactiveColour, indicatorLerpage);
			leftArrow.color = Color.Lerp(leftArrow.color, inactiveColour, indicatorLerpage);
		}
		velocity = Move(velocity, input);
		
		if (grounded || (velocity.y < 0 && Mathf.Abs(t.position.y - jumpInitHeight) < multiJumpWindow)){		//jump if I'm grounded, or if I meet double jump requirements
			if (jumpTimer < maxPress2Jump){
				//do the jump! Add the last part there to give myself a running jump
				this.Jump(initialJumpVelocity + Mathf.Abs(velocity.x/runningJumpModifier));
				jumpInitHeight = t.position.y;
			}
		}
		base.Update();
	}
	
	void LateUpdate () {
		Vector3 forepos = t.position;		//here I'll record my position and see if I've moved later, to create an after-image for great JUICE
		base.LateUpdate();
		if (t.position != forepos){
			Instantiate(Resources.Load("afterimage"), t.position, t.rotation);
		}
	}
	
}
