﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RghtButton : MonoBehaviour {
	StateMachine gameManager;
	public int BoonChoiceState;
	// Use this for initialization
	void Start () {
		BoonChoiceState = (int)Mathf.Floor(Random.Range(0,2));
		string[] Boons = {"More damage","More HP","Faster Movement"};
		string corrispondingBoon = Boons[BoonChoiceState];
		this.GetComponentInChildren<Text>().text = ("Or Be granted "+ corrispondingBoon + "?");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void onClick(){
		string[] NumToString = {"q","w","e","a","s","d","space"}  ;
		//Trade(NumToString[SacChoiceState]);
		//onTradeFinish();
	}
}