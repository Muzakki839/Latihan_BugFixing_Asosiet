﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countdownPage;
	public GameObject woodsSpawner;
	public Text scoreText;

	enum PageState
	{
		None,
		Start,
		GameOver,
		Countdown
	}

	int score = 0;
	bool gameOver = true;

	public bool GameOver { get { return gameOver; } }

	void Awake()
	{

		Instance = this;
	}

	void OnEnable()
	{
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
		CountdownText.OnCountdownFinished += OnCountdownFinished;

	}

	void OnDisable()
	{
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;

	}

	void OnCountdownFinished()
	{
		SetPageState(PageState.None);
		score = 0;
		OnGameStarted();
		gameOver = false;
		// Debug.Log(gameOver);

	}

	void OnPlayerDied()
	{
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt("highscore");
		if (score < savedScore)
		{
			PlayerPrefs.SetInt("highscore", score);

		}
		SetPageState(PageState.GameOver);
	}

	void OnPlayerScored()
	{
		int _score = 0;
		_score++;
		scoreText.text = _score.ToString();
	}

	void SetPageState(PageState state)
	{
		switch (state)
		{

			case PageState.None:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(false);
				woodsSpawner.SetActive(true);
				break;
			case PageState.Start:
				startPage.SetActive(true);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(false);
				woodsSpawner.SetActive(false);
				break;
			case PageState.GameOver:
				startPage.SetActive(false);
				gameOverPage.SetActive(true);
				countdownPage.SetActive(false);
				woodsSpawner.SetActive(false);
				break;
			case PageState.Countdown:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countdownPage.SetActive(true);
				woodsSpawner.SetActive(false);
				break;

		}
	}

	public void ConfirmedGameOver()
	{
		OnGameOverConfirmed();
		scoreText.text = "0";
		SetPageState(PageState.Start);
	}

	public void StartGame()
	{
		SetPageState(PageState.Countdown);
	}
}
