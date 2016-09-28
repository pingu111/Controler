using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour
{
    public int idSpike = 0;

	// Use this for initialization
	void Start ()
    {
        subscribeEvent<int>();
    }
	
    private void subscribeEvent<T>()
    {
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEOUT, raiseSpike);
    }

    public void raiseSpike(int i)
    {
        if (i == idSpike)
            raiseThisSpike();
    }

    private void raiseThisSpike()
    {
        ;
    }
}
