using UnityEngine;
using System.Collections.Generic;

public enum TypeScriptedEvent
{
    SPIKEIN,
    SPIKEOUT,
    TEXT
}

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// The player to destroy if game over
    /// </summary>
    public GameObject player;

    /// <summary>
    /// List of the events that we'll raise during runtime
    /// </summary>
    public List<ScriptedEvent> listEvenets = new List<ScriptedEvent>();

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
