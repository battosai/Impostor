﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
	//public read-only constants
	public int winScore {get{return 3;}}
	public int gameRoundCount {get{return 7;}}
	public static List<int> selectCount {get{return new List<int> {3, 4, 1, 5, 5, 1, 5};}}
	//public attributes, but only changeable within class
	public int playerScore {get; private set;}
	public int defectorScore {get; private set;}
	public static int gameRound {get; private set;}
	//private attributes
	private Button proceedButton;
	private Grid grid;

	void Awake()
	{
		grid = GameObject.Find("Grid").GetComponent<Grid>();
		proceedButton = GameObject.Find("ProceedButton").GetComponent<Button>();
	}

	// Use this for initialization
	void Start()
	{
		grid.resetGrid();
		gameRound = 0;
		proceedButton.onClick.AddListener(proceedToNextRound);
	}

	// Update is called once per frame
	void Update()
	{
		checkSelectionCount();
	}

	//disables/enables proceed button if enough selections are made
	private void checkSelectionCount()
	{
		if(Grid.selectedHeads.Count == selectCount[gameRound])
		{
			proceedButton.interactable = true;
		}
		else
		{
			proceedButton.interactable = false;
		}
	}

	//checks conditions for current round and advances to the next one
	private void proceedToNextRound()
{
		if(selectCount[gameRound] > 1)
		{
			//mission round currently
			bool isDefectorPresent = false;
			foreach(GameObject head in Grid.selectedHeads)
			{
				 isDefectorPresent = isDefectorPresent || head.GetComponent<Head>().isDefector);
			}
			if(isDefectorPresent)
				defectorWinRound();
			else
				playerWinRound();
		}
		else
		{
			//interrogation round currently
			GameObject currentHead = Grid.selectedHeads[0];
			if(currentHead.GetComponent<Head>().isDefector)
				Debug.Log("THIS IS A DEFECTOR");
			else
				Debug.Log("THIS IS AN ALLY");
		}
		if(checkEndGame())
			Debug.Log("GAME OVER!");
		grid.resetSelectedHeads();
		gameRound++;
	}

	//checks end game conditions
	private bool checkEndGame()
	{
		if(playerScore == winScore)
			Debug.Log("Allies win!");
		if(defectorScore == winScore)
			Debug.Log("Defectors win!");
	}

	//call when player wins round
	private void playerWinRound()
	{
		playerScore++;
		//kill a head
		int killIndex = Random.Range(0, Grid.selectedHeads.Count);
		GameObject currentHead = Grid.selectedHeads[killIndex];
		currentHead.killHead();
	}

	//call when defectors win round
	private void defectorWinRound()
	{
		defectorScore++;
	}
}
