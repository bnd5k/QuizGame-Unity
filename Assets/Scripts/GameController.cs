﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Text questionDisplayText;
	public SimpleObjectPool answerButtonObjectPool;
	public Transform answerButtonParent;
	public Text scoreDisplayText;
	public Text timeRemainingDisplayText;

	public GameObject questionDisplay;
	public GameObject roundEndDisplay;
	public Text highScoreDisplay;

	private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;
	private bool isRoundActive;
	private float timeRemaining;
	private int questionIndex;
	private int playerScore;
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();


	void Start () {
		dataController = FindObjectOfType<DataController> ();
		currentRoundData = dataController.GetCurrentRoundData ();
		questionPool = currentRoundData.questions;
		timeRemaining = currentRoundData.timeLimitInSeconds;
		UpdateTimeRemainingDisplay ();

		playerScore = 0;
		questionIndex = 0;

		ShowQuestion ();
		isRoundActive = true;

	}

	private void ShowQuestion()
	{
		RemoveAnswerButtons ();
		QuestionData questionData = questionPool [questionIndex];
		questionDisplayText.text = questionData.questionText;

		for (int i = 0; i < questionData.answers.Length; i++) {
			GameObject answerButtonGameObject = answerButtonObjectPool.GetObject ();
			answerButtonGameObjects.Add(answerButtonGameObject);
			answerButtonGameObject.transform.SetParent (answerButtonParent);


			AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton> ();
			answerButton.Setup (questionData.answers[i]);
		}
	}

	private void RemoveAnswerButtons() {
		while (answerButtonGameObjects.Count > 0) {
			answerButtonObjectPool.ReturnObject (answerButtonGameObjects [0]);
			answerButtonGameObjects.RemoveAt (0);
		}
	}

	public void AnswerButtonClicked(bool isCorrect) {
		if (isCorrect) {
			playerScore += currentRoundData.pointsAddedForCorrectAnswer;
			scoreDisplayText.text = "Score: " + playerScore.ToString();
		}

		if (questionPool.Length	 > questionIndex + 1) {
			questionIndex++;
			ShowQuestion();
		} else {
			EndRound ();
		}
	}

	public void EndRound() {
		isRoundActive = false;


		dataController.SubmitNewPlayerScore(playerScore);
		Debug.Log($"{dataController.GetHighestPlayerScore().ToString()}");
		highScoreDisplay.text = "High Score: " + dataController.GetHighestPlayerScore ().ToString ();

		questionDisplay.SetActive (false);
		roundEndDisplay.SetActive (true);
	}

	public void ReturnToMenu(){
		SceneManager.LoadScene ("MenuScreen");
	}

	private void UpdateTimeRemainingDisplay() {
		timeRemainingDisplayText.text = "Time: " + Mathf.Round (timeRemaining).ToString (); 
	}

	void Update () {
		if (isRoundActive) {
			timeRemaining -= Time.deltaTime;
			UpdateTimeRemainingDisplay ();

			if (timeRemaining <= 0f) {
				EndRound ();
			}
		}
	}
}
