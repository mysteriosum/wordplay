using UnityEngine;
using System;
using System.Collections;

static public class Textf {

	
	static public string GangsterWrap(string longstring,int[] lineLengths){
		int currentLine = 0;
    	int charCount = 0;
		char[] delimiterChars = { ' ' };
		String[] words = longstring.Split(delimiterChars);
    	String result = "";
		int lengthsIndex = 0;
		
 	    for (int index = 0; index < words.Length; index++) {
        	string word = words[index];

        	if (index == 0) {
            	result = word;
				charCount = word.Length;
            }
			else if (index > 0 ) {
            	charCount += word.Length + 1; //+1 because we assume that there will be a space after every word
            	if (charCount <= lineLengths[lengthsIndex]) {
					result += " " + word;
				}
				else {
                	charCount = word.Length + 1;
	                result += Environment.NewLine + word;
					currentLine ++;
					lengthsIndex = currentLine >= lineLengths.Length? lengthsIndex : lengthsIndex + 1;
            	}

            }
        }
		return result;
    }
	
	static public string GangsterWrap(string longstring, int lineLengths){
		return GangsterWrap (longstring, new int[] { lineLengths });
	}
	
}
