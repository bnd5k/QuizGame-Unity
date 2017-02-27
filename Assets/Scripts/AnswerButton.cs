using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnswerButton : MonoBehaviour {

	public Text answerText;

	private AnswerData answerData;

	// Use this for initialization
	void Start () {
		
	}

	public void Setup(AnswerData data)
	{
		answerData = data;
		answerData.answerText = answerData.answerText;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
