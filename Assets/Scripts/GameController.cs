using System.Collections;
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
	public GameObject questionDisplay;
	public GameObject roundEndDisplay;

	private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;

	private bool isRoundActive;
	private float timeRemaining;
	private int questionIndex;
	private int playerScore;
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();


	void Start () {
		Debug.Log("Inside GameController.start");
		dataController = FindObjectOfType<DataController> ();
		currentRoundData = dataController.GetCurrentRoundData ();
		Debug.Log($"{currentRoundData.questions}");
		questionPool = currentRoundData.questions;
		timeRemaining = currentRoundData.timeLimitInSeconds;

		Debug.Log ($"{timeRemaining}");

		playerScore = 0;
		questionIndex = 0;

		ShowQuestion ();
		isRoundActive = true;

		Debug.Log ("At end of GameController.start");
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

		if (questionPool.Length > questionIndex + 1) {
			questionIndex++;
			ShowQuestion();
		} else {
			EndRound ();
		}
	}

	public void EndRound() {
		isRoundActive = false;
		questionDisplay.SetActive (false);
		roundEndDisplay.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
