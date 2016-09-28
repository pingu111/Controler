using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// The prefab of the spikes
    /// </summary>
    public GameObject spikePrefab;

	// Use this for initialization
	void Start ()
    {
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEIN, test);

        EventManager.raise<int>(MyEventTypes.SPIKEIN, 12);

    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    void test(int a)
    {
        Debug.Log(a);
    }
}
