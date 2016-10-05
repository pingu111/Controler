﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    /// <summary>
    /// The text with the score in it
    /// </summary>
    public GameObject textScore;

    /// <summary>
    /// The text with the instruction : Go, jump...
    /// </summary>
    public GameObject textInstructions;

    /// <summary>
    /// The score of the player
    /// </summary>
    private int score = 0;

    /// <summary>
    /// Has the game started ?
    /// </summary>
    bool gameStartedAndPlayerAlive = false;

    /// <summary>
    /// Number of points gained per frame
    /// </summary>
    [SerializeField]
    private int pointsPerFrame = 1;

	// Use this for initialization
	void Start ()
    {
        textScore.GetComponent<Text>().text = "";
        textInstructions.GetComponent<Text>().text = "";
        gameStartedAndPlayerAlive = false;
        EventManager.addActionToEvent<int>(MyEventTypes.SCOREADD, addScore);
        EventManager.addActionToEvent<int>(MyEventTypes.TEXTCHANGE, textChange);
        EventManager.addActionToEvent(MyEventTypes.ONLOSE, playerLost);

    }

    void Update()
    {
        if(gameStartedAndPlayerAlive)
        {
            addScore(pointsPerFrame);
        }
    }

    void playerLost()
    {
        gameStartedAndPlayerAlive = false;
    }

    /// <summary>
    /// Add and print the score
    /// </summary>
    /// <param name="scoreToAdd"></param>
    void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        textScore.GetComponent<Text>().text = score.ToString("n0");
    }

    /// <summary>
    /// Change the text with some IDs
    /// </summary>
    /// <param name="idText"></param>
    void textChange(int idText)
    {
        switch(idText)
        {
            case 1:
                textInstructions.GetComponent<Text>().text = "3";
                break;
            case 2:
                textInstructions.GetComponent<Text>().text = "2";
                break;
            case 3:
                textInstructions.GetComponent<Text>().text = "1";
                break;
            case 4:
                textInstructions.GetComponent<Text>().text = "GO !";
                gameStartedAndPlayerAlive = true;
                break;
            case 5:
                textInstructions.GetComponent<Text>().text = "Attention...";
                break;
            case 6:
                textInstructions.GetComponent<Text>().text = "Bon courage !";
                break;
            default:
                textInstructions.GetComponent<Text>().text = "";
                break;
        }
    }

}
