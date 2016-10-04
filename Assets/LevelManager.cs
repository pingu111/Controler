using UnityEngine;
using System.Collections.Generic;

public enum TypeScriptedEvent
{
    SPIKEIN,
    GROUPSPIKEIN,

    SPIKEOUT,
    GROUPSPIKEOUT,

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

        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 2, 1));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 3, 2));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 4, 3));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 5, 4));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 6, 5));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 7, 6));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 8, 7));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 9, 8));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 10, 9));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 11, 10));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 12, 11));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 13, 12));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 14, 13));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 15, 14));



        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 15, 1));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 16, 2));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 17, 3));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 18, 4));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 19, 5));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 20, 6));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 21, 7));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 22, 8));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 23, 9));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 24, 10));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 25, 11));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 26, 12));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 27, 13));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 28, 14));

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

                    case TypeScriptedEvent.GROUPSPIKEIN:
                        EventManager.raise<int>(MyEventTypes.GROUPSPIKEIN, id);
                        break;
                    case TypeScriptedEvent.GROUPSPIKEOUT:
                        EventManager.raise<int>(MyEventTypes.GROUPSPIKEOUT, id);
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
