using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TextCollection : MonoBehaviour {
	
	private Transform t;
	private List<WordStructure> words = new List<WordStructure>();
	private TextMesh mesh;
	private List<TextMesh> storyMeshes = new List<TextMesh>();
	private String[] delimitors = new String[] { Environment.NewLine };
	
	private static String myText = "";
	private static String storyLong = "";
	
	private float spacing = 3.4f;
	private int lineLength = 30;
	private float zIncrement = 2f;
	private float alphaIncrement = 0.25f;
	private SentenceStructure mySentence;
	
	public delegate void ResetDelegate();
	
	public event ResetDelegate reset;
	
	private static TextCollection instance;
	
	private float resetTimer = 2.0f;
	private bool fading = false;
	private bool faded = false;
	private float fadeSpeed = 0.1f;
	private Color initColor;
	private bool finished = false;
	private int maxLinesShown = 8;
	
	//timer variables (text roll)
	private int addTimer = 0;
	private int addTiming = 5;
	
	private TextCollectible[] allWords;
	private int longestWord;
	
	public static TextCollection Instance {
		get {
			return instance;
		}
	}
	
	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	void Start () {
		t = transform;
		mesh = GetComponent<TextMesh>();
		mesh.text = myText;
		initColor = mesh.renderer.material.color;
		mesh.renderer.material = Resources.Load("Textymat") as Material;
		//here I set the text shader to the same render order as geometry, so it can interact with it properly
		//mesh.renderer.material.renderQueue = 1000;
		//mesh.renderer.material.color = Color.black;
		mySentence = SentenceStructure.GetSentence(Application.loadedLevelName);
		
		allWords = FindSceneObjectsOfType(typeof(TextCollectible)) as TextCollectible[];
		
		foreach(String sentence in mySentence.acceptables){
			int length = sentence.Split(new char[] {' '}).Length;
			if (length > longestWord){
				longestWord = length;
			}
		}
		
		
		//assign & create my meshes
		String storyLines = Textf.GangsterWrap(storyLong, lineLength);
		String[] storySplit = storyLines.Split(delimitors, StringSplitOptions.None);
		
		for (int i = 0; i < storySplit.Length; i ++){
			GameObject newLine = Instantiate(Resources.Load("textmesh_empty"), t.position, t.rotation) as GameObject;
			TextMesh newMesh = newLine.GetComponent<TextMesh>();
			
			newMesh.text = storySplit[i];		//add the text to the mesh
			newMesh.offsetZ = (storySplit.Length - i) * zIncrement;
			newMesh.renderer.material.color *= ((maxLinesShown - storySplit.Length + i) * alphaIncrement);
			newMesh.renderer.material = Resources.Load("Textymat") as Material;
			
			storyMeshes.Add(newMesh);		//assign the meshes to my list
			newLine.transform.Translate(Vector3.up * spacing * (storySplit.Length - (i)));	//move the strings in the correct arrangement
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (reset != null && Input.GetKey(KeyCode.R) && !finished && faded){
			Restart();
		}
		
		if (!faded && !fading){
//			if (Input.GetButton("Jump")){
				fading = true;
//			}
		}
		
		if (fading && !faded){
			mesh.renderer.material.color = Vector4.Lerp(mesh.renderer.material.color, new Color(1f, 1f, 1f, 0f), fadeSpeed);
			
			if (storyMeshes.Count > 0){
				storyMeshes[storyMeshes.Count - 1].renderer.material.color = new Color(1f, 1f, 1f, 1 - mesh.renderer.material.color.a);
			}
			
			if (mesh.renderer.material.color == Color.clear){
				ResetMesh();
			}
		}
		
		//add a character every [addTimer] frames
		if (!String.Equals(mesh.text, myText)){
			addTimer ++;
			if (addTimer == addTiming){
				mesh.text += myText[mesh.text.Length];
				addTimer = 0;
			}
		}
	}
	
	private void ResetMesh (){
		faded = true;
		fading = false;
		myText = "";
		mesh.text = "";
		mesh.renderer.material.color = initColor;
	}
	
	public void AddWord (TextCollectible text){
		
		if (words.Count == 0){
			ResetMesh();
		}
		
		words.Add(text.word);
		if (words.Count > 1)
			myText += " ";
		myText += text.Text;
		reset += text.Reset;
		
		for (int i = 0; i < mySentence.acceptables.Length; i ++){
			if (String.Equals(myText, mySentence.acceptables[i])){		//if what the player has so far is good, accept it
				Feedback.instance.Approve();
				WordCamera.instance.End(mySentence.targets[i]);		//this is where the next level is loaded
				finished = true;
				Quill.player.enabled = false;
				
				storyLong += myText + ". ";			//update my long story-text, adding appropriate punctuation.
				if (mySentence.paragraphEnd)
					storyLong += Environment.NewLine;		//NEW: Add a linebreak if it is one
				
				return;
			}
		}
		if (words.Count >= longestWord){
			Feedback.instance.Disapprove();
		}
	}
	
	void Restart () {
		myText = "";
		mesh.text = "";
		words.Clear();
		reset();
		reset = null;
		Quill.player.enabled = true;
		reset += Quill.player.Restart;
	}
}
//------------------------------------------other classes---------------------------------------------


/*
public static class Paths {
	
	public static SentenceStructure GoToNext (SentenceStructure previous, String submitted){
		int index = 0;
		foreach (String s in previous.acceptables){
			if (String.Equals(submitted, previous.acceptables[index], StringComparison.OrdinalIgnoreCase)){
				break;
			}
			index ++;
		}
		
		String next = previous.targets[index];
		
		foreach (SentenceStructure ss in SentenceStructure.allSentences){
			if (String.Equals(ss.name, next, StringComparison.OrdinalIgnoreCase)){
				return ss;
			}
		}
		
		return null;
		
	}
	
	public static String[] GetAcceptables(String level){
		switch (level){
		case "It's after the assembly":
			return new String[] { "It's after the assembly" };
			
		case "I love you":
			return new String[] { "I love you", "I'm too afraid to speak" };
			
			
		default:
			return new String[] { level };
		}
	}
	
	public static String[] GetTargets(String level){
		
		switch (level){
			
		case "It's after the assembly":
			return new String[] { "She passes me" };
			
		case "She passes me":
			return new String[] { "She's so close" };
			
		case "She's so close":
			return new String[] { "I love you" };
			
		case "I love you":
			return new String[] { "It wasn't easy, but I did it.", "To my tribe I am a man" };
			
		case "To my tribe I am a man":
			return new String[] { "I killed a lion" };
			
		case "I killed a lion":
			return new String[] { "She is more daunting" };
			
		case "She is more daunting":
			return new String[] { "Father cannot understand" };
			
		case "Father cannot understand":
			return new String[] { "He says it's easy" };
			
		case "He says it's easy":
			return new String[] { "He had Mary at my age" };
			
		case "He had Mary at my age":
			return new String[] { "I am timid like Mother" };
			
		case "I am timid like Mother":
			return new String[] { "If only I could rescue her from danger" };
			
		case "If only I could rescue her from danger":
			return new String[] { "I could show her how I feel" };
			
		case "I could show her how I feel":
			return new String[] { "Show her I would do anything for her" };
			
			
			
		default:
			return new String[] { "" };
		}
	}
	
}*/

//-------------------------------------Things that I started doing but thought it would be too complicated....-----------------------------
public class LanguageRules{
	public class WordTypes {
		public String pronoun = "pronoun";
		public String noun = "noun"; 
		public String verb = "verb"; 
		public String adjective = "adjective";
		public String adverb = "adverb";
		public String conjunction = "conjunction";
		public String article = "article";
		public String preposition = "preposition";
		public String interjection = "interjection";
	}
	
	public static WordTypes types = new WordTypes();
	
	public class ClauseStructure {
		public String name;
		public String[,] acceptables;
		public ClauseStructure (String name, String[,] acceptables){
			this.name = name;
			this.acceptables = acceptables;
		}
	}
	
	
	
	private String[,] nounAcceptables = 
		new String[,]{
		{
			types.noun,
		},
		{
			types.adjective,
			types.noun,
		},
		{
			types.article,
			types.noun,
		},
		{
			types.article,
			types.adjective,
			types.noun,
		},
		{	
			types.article,
			types.adverb,
			types.adjective,
			types.noun,
		},
		{
			types.noun,
		},
		{
			types.noun,
		},
	};
	public ClauseStructure nounClause;
	public ClauseStructure verbClause;
	public ClauseStructure adjectiveClause;
	public ClauseStructure adverbClause;
	
	
	public LanguageRules () {
		nounClause = new ClauseStructure("noun clause", nounAcceptables);
	}
	
	private static LanguageRules rules;
	public static LanguageRules Rules {
		get {
			if (rules == null)
				rules = new LanguageRules();
			return rules;
		}
	}
	
}

public class WordStructure {
	public String word;
	public String[] roles;
	
	public WordStructure (String word, String[] roles) {
		this.word = word;
		this.roles = roles;
	}
}
