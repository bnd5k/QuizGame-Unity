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
		answerText.text = answerData.answerText;
		Debug.Log ($"{answerData.answerText}");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
