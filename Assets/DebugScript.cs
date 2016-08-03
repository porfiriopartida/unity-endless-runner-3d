using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DebugScript : MonoBehaviour {
	public static DebugScript self;
	private ArrayList texts = new ArrayList();
	private Text text;
	public void Awake(){
		self = this;

		text = GetComponent<Text>();
		text.text = "GG EZ";
	}
	public void addText(string textValue){
		texts.Add(textValue);
		int size = texts.Count;

		if(size > 3){
			texts.RemoveAt(0);
		}
		size = texts.Count;
		string newText = "";
		for(int i = size - 1; i >= 0 ; i--){
			newText +=  texts[i] + Environment.NewLine;
		}

		text.text = newText;
	}
	// Update is called once per frame
	void Update () {
	}
}
