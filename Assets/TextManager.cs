using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour {

    public GameObject textScore;

    public GameObject textInstructions;


    private int score = 0;

	// Use this for initialization
	void Start ()
    {
        EventManager.addActionToEvent<int>(MyEventTypes.SCOREADD, addScore);
        EventManager.addActionToEvent<int>(MyEventTypes.TEXTCHANGE, textChange);

    }

    void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    void textChange(int idText)
    {
        switch(idText)
        {
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

}
