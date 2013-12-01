using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum PunctuationEnum {
	fullStop = 0,
	exclamation = 1,
	question = 2,
	rage = 3,
	colon = 4,
	semicolon = 5,
	ellipse = 6,
	endEllipse = 7,
}

public class SentenceStructure {
	
	public String name;
	public String defaultTarget;
	public String[] targets; 			//levels I can lead to
	public String[] acceptables;		//sentences I'll accept. They should correspond with the targets (same index styles)
	public bool paragraphEnd = false;	//whether this sentence should be the end of a paragraph
	public PunctuationEnum punctuation = PunctuationEnum.fullStop; 
	
	public String PunctString {
		get {
			String s;
			switch (punctuation){
			case PunctuationEnum.colon:
				return ":";
			case PunctuationEnum.exclamation:
				return "!";
			case PunctuationEnum.fullStop:
				return ".";
			case PunctuationEnum.question:
				return "?";
			case PunctuationEnum.semicolon:
				return ";";
			case PunctuationEnum.rage:
				return "!!!";
			case PunctuationEnum.ellipse:
				return "...";
			case PunctuationEnum.endEllipse:
				return "....";
			default:
				return "punctuationError";
			}
		}
	}
	
	public SentenceStructure (String name, String target){
		this.name = name;
		this.targets = new String[] { target };
		this.defaultTarget = target;
		this.acceptables = new String[] { name };
		
		//allSentences.Add (this);
	}
	
	public SentenceStructure (String name, String[] targets, String[] acceptables){
		this.name = name;
		this.targets = targets;
		this.defaultTarget = targets[0];
		this.acceptables = acceptables;
		
		//allSentences.Add (this);
	}
	
	public SentenceStructure (String name, String[] targets, String[] acceptables, bool paragraphEnd, PunctuationEnum punctuation){
		this.name = name;
		this.targets = targets;
		this.defaultTarget = targets[0];
		this.acceptables = acceptables;
		this.paragraphEnd = paragraphEnd;
		this.punctuation = punctuation;
		
		//allSentences.Add (this);
	}
	
	public static SentenceStructure GetSentence (String level){
		foreach (SentenceStructure ss in allSentences){
			if (ss.name == level){
				return ss;
			}
		}
		Debug.LogWarning("Dude, this sentence is not in my sentence structures: " + level);
		return null;
	}
	
	//public static List<SentenceStructure> allSentences = new List<SentenceStructure>();
	public static SentenceStructure[] allSentences = new SentenceStructure[] { 
		
		new SentenceStructure(
			"It's after the assembly",
			"She passes me"
		),
		
		new SentenceStructure(
			"She passes me", 
			"She's so close"
		),
		
		new SentenceStructure(
			"She's so close", 
			"I love you"
		),
		
		new SentenceStructure(
			"I love you",
			new String[] { 
				"To my tribe I am a man", 
				"I don't know what came over me"
			},
			new String[] { 
				"I'm too afraid to speak", 
				"I love you" }
		),
		
		new SentenceStructure(
			"To my tribe I am a man", 
			"I killed a lion"
		),
		
		new SentenceStructure(
			"I killed a lion", 
			"She is more daunting"
		),
		
		new SentenceStructure(
			"She is more daunting", 
			new String[] { "Father cannot understand" }, 
			new String[] { "She is more daunting" }, 
			true, 
			PunctuationEnum.fullStop
		),
		
		new SentenceStructure(
			"Father cannot understand",
			"He had Mary at my age"
		),
		
		new SentenceStructure(
			"He had Mary at my age",
			"I am timid like Mother"
		),
		
		new SentenceStructure(
			"I am timid like Mother",
			"If only I could rescue her"
		),
		
		new SentenceStructure(
			"If only I could rescue her", 
			new String[] { "I could show her how I feel" }, 
			new String[] { "If only I could rescue her" }, 
			false, 
			PunctuationEnum.endEllipse
		),
		
		new SentenceStructure(
			"I could show her how I feel",
			"But life's not so simple"
		),
		
		new SentenceStructure(
			"But life's not so simple",
			"Society has rules"
		),
		
		new SentenceStructure(
			"Society has rules",
			"I will woo with words or not at all"
		),
		
	};
	
}