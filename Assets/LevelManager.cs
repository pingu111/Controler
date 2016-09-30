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
    public List<ScriptedEvent> listEvents = new List<ScriptedEvent>();

    /// <summary>
    /// Time at the beginning of the events
    /// </summary>
    private float timeBeginning = 0;

	// Use this for initialization
	void Start ()
    {
        EventManager.addActionToEvent(MyEventTypes.ONLOSE, onLose);

        ScriptedEvent eventA = new ScriptedEvent(TypeScriptedEvent.SPIKEOUT, 2, 10);
        ScriptedEvent eventB = new ScriptedEvent(TypeScriptedEvent.SPIKEOUT, 3, 15);
        ScriptedEvent eventC = new ScriptedEvent(TypeScriptedEvent.SPIKEOUT, 2, 5);
        ScriptedEvent eventD = new ScriptedEvent(TypeScriptedEvent.SPIKEIN, 10, 10);
        ScriptedEvent eventE = new ScriptedEvent(TypeScriptedEvent.SPIKEIN, 10, 15);
        ScriptedEvent eventF = new ScriptedEvent(TypeScriptedEvent.SPIKEOUT, 10, 50);

        listEvents.Add(eventA);
        listEvents.Add(eventB);
        listEvents.Add(eventC);
        listEvents.Add(eventD);
        listEvents.Add(eventE);
        listEvents.Add(eventF);

    }

    // Update is called once per frame
    void Update ()
    {
        // initialisation of the time
        if (timeBeginning == 0)
            timeBeginning = Time.time;

        // we check the events
        for (int i = (listEvents.Count - 1); i >= 0; --i)
        {
            if ((Time.time - timeBeginning) >= listEvents[i].timeToLaunchEvent)
            {
                int id = listEvents[i].id;
                switch (listEvents[i].typeOfEvent)
                {
                    case TypeScriptedEvent.SPIKEIN:
                        EventManager.raise<int>(MyEventTypes.SPIKEIN, id);
                        break;
                    case TypeScriptedEvent.SPIKEOUT:
                        EventManager.raise<int>(MyEventTypes.SPIKEOUT, id);
                        break;
                    case TypeScriptedEvent.TEXT:
                        break;
                    default:
                        break;
                }

                listEvents.Remove(listEvents[i]);
            }
        }
	}

    void onLose()
    {
        Debug.Log("Aha loser");
        Destroy(player);
    }
}
