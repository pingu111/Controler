using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public GameObject player;

	// Use this for initialization
	void Start ()
    {
        EventManager.addActionToEvent(MyEventTypes.ONLOSE, onLose);

    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    void onLose()
    {
        Debug.Log("Aha loser");
        Destroy(player);
    }
}
