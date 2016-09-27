using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.addActionToEvent(MyEventTypes.SPIKEOUT, getout);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void getout(int i)
    {

    }
}
