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

    /// <summary>
    /// Cooldown between the spikes
    /// </summary>
    private float cooldown = 2;

    /// <summary>
    /// The total number of group of spikes
    /// </summary>
    private int nbGroupSpikes = 4;

    /// <summary>
    /// The total number of spikes
    /// </summary>
    private int nbSpikes = 4;

    /// <summary>
    /// The creator of the spikes
    /// </summary>
    public GameObject spikeFactory;

    // Use this for initialization
    void Start ()
    {
        EventManager.addActionToEvent(MyEventTypes.ONLOSE, onLose);
        createListEventsScripted();

        nbSpikes = (spikeFactory.GetComponent<SpikeFactory>().nbSpikesGround + 2*spikeFactory.GetComponent<SpikeFactory>().nbSpikesWall);
        nbGroupSpikes = (spikeFactory.GetComponent<SpikeFactory>().nbGroupGround + 2 * spikeFactory.GetComponent<SpikeFactory>().nbGroupWall);
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
                        EventManager.raise<int>(MyEventTypes.TEXTCHANGE, id);
                        break;

                    default:
                        break;
                }
                listEvents.Remove(listEvents[i]);
            }
        }
        if(listEvents.Count < 1 )
        {
            float nextTime = ((Time.time - timeBeginning) + cooldown);
            int randomId = 0;

            if (Random.Range(0,100) < 80)
            {
                randomId =(int)Random.Range(1, nbGroupSpikes - 0.01f);
                listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, Time.time+cooldown, randomId));
                listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, Time.time + cooldown + 4, randomId));
            }
            else
            {
                randomId = (int)Random.Range(1, nbSpikes - 0.01f);
                listEvents.Add(new ScriptedEvent(TypeScriptedEvent.SPIKEIN, Time.time + cooldown, randomId));
                listEvents.Add(new ScriptedEvent(TypeScriptedEvent.SPIKEOUT, Time.time + cooldown + 4, randomId));
            }
        }
    }

    void createListEventsScripted()
    {
        // Text "3,2,1, go"
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 1, 1));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 2, 2));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 3, 3));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 4, 4));

        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 6, 5));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 6, 1));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 10, 1));

        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEOUT, 9, 6));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.GROUPSPIKEIN, 13, 6));

        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 12, 6));
        listEvents.Add(new ScriptedEvent(TypeScriptedEvent.TEXT, 15, 42));
    }

    void onLose()
    {
        Debug.Log("Aha loser");
        Destroy(player);
    }
}
