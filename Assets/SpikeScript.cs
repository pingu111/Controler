using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
    private void testEvent<T>()
    {
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEOUT, getout);
        EventManager.raise<int>(MyEventTypes.SPIKEOUT, 12);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void getout(int i)
    {
        Debug.Log(i);
    }
}
