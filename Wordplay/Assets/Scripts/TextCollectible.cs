using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TextCollectible : MonoBehaviour {
	
	private TextMesh mesh;
	private Transform t;
	private Renderer r;
	private BoxCollider bc;
	private Material mat;
	private Color initCol = Color.white;
	private Color collectedCol = new Color(0.05f, 0.05f, 0.05f, 0.75f);
	
	private bool collected = false;
	
	public bool Collected {
		get { return collected; }
		set { collected = value; }
	}
	
	public String Text{
		get { return mesh.text; }
	}
	public WordStructure word;
	
	//little hack because apparently they suck at placing box colliders correctly
	private float bcOffset = 0.5f;
	private float lerpAmount = 0.05f;
	
	// Use this for initialization
	
	void Awake () {
		//this is in awake because if I want to put a Movable script on something I have to have this happen first
		bc = gameObject.AddComponent<BoxCollider>();
		bc.center = new Vector3(bc.center.x, bc.center.y - bcOffset, bc.center.z); //this hack is to get the box collider in the right place.
		bc.isTrigger = true;
		
	}
	void Start () {
		mesh = GetComponent<TextMesh>();
		mesh.text = Text;
		mesh.renderer.material = Resources.Load("Textymat") as Material;
		
		
		r = mesh.renderer;
		
		t = transform;
		
		mat = mesh.renderer.material;
		
		TextColor col = GetComponent<TextColor>();
		if (col){
			initCol = col.colour;
		}
		
		//word = Words.GetStructure(Text);
		
	}
	
	// Update is called once per frame
	void Update () {
		if (collected && mat.color != collectedCol){
			mat.color = Vector4.Lerp(mat.color, collectedCol, lerpAmount);
		}
		else if (!collected && mat.color != initCol){
			mat.color = Vector4.Lerp(mat.color, initCol, lerpAmount);
		}
	}
	
	void OnTriggerEnter (Collider other){
		if (this.Collected) return;
		if (other.tag != "Player") return;
		
		TextCollection.Instance.AddWord(this);
		this.Collected = true;
	}
	
	public void Reset () {
		Collected = false;
	}
	
	public void SetText(String newText){
		if (!mesh)
			mesh = GetComponent<TextMesh>();
		mesh.text = newText;
	}
}

public class Words {
	
	WordStructure[] words;
	
	public Words (){
		words = new WordStructure[] {
			new WordStructure(
				"hello", new String[] { LanguageRules.types.interjection, LanguageRules.types.noun }
			),
			new WordStructure(
				"world", new String[] { LanguageRules.types.noun, LanguageRules.types.adjective }
			),
			new WordStructure(
				"how", new String[] { LanguageRules.types.adverb, LanguageRules.types.conjunction }
			),
			new WordStructure(
				"are", new String[] { LanguageRules.types.verb }
			),
			new WordStructure(
				"you", new String[] { LanguageRules.types.pronoun }
			),
			
		};
		
	}
	
	private static Words instance;
	public static Words Instance {
		get {
			if (instance == null)
				instance = new Words();
			return instance;
		}
	}
	
	static public WordStructure GetStructure (String text){
		foreach (WordStructure word in Instance.words){
			if (String.Equals(text, word.word, StringComparison.OrdinalIgnoreCase))
				return word;
		}
		
		Debug.LogWarning("There's no way I'm giving you a word. It's not on my list! You have to define things on my list!");
		return new WordStructure("fuck", new String[] {LanguageRules.types.interjection});
	}
}