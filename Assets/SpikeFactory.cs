using UnityEngine;
using System.Collections.Generic;

public class SpikeFactory : MonoBehaviour
{
    /// <summary>
    /// The prefab to build the spikes
    /// </summary>
    public GameObject spikePrefab;

    /// <summary>
    /// List of the position of the spikes
    /// Position : betweeen 0 and 1
    /// </summary>
    /// 
    public List<Vector3> posSpike;

    /// <summary>
    /// List of the scale of the spikes
    /// Scale : whatever you want
    /// </summary>
    public List<Vector3> scaleSpikes;

    // Use this for initialization
    void Start ()
    {
        // XML read of the spikes ?

        if (posSpike.Count != scaleSpikes.Count)
            Debug.Log("CRITICAL pos != scale");

        initSpikes();
	}
	
    /// <summary>
    /// Initialize the walls, ground, and roof
    /// </summary>
    void initSpikes()
    {
        //We create all the spikes
        for(int i = 0; i < posSpike.Count; i ++)
        {
            GameObject spike = Instantiate(spikePrefab);
            int id = i + 1;
            spike.GetComponent<SpikeScript>().idSpike = id;
            spike.transform.parent = this.transform;

            Vector3 posSpikeWorld = Camera.main.ViewportToWorldPoint(posSpike[i]);
            spike.transform.position = new Vector3(posSpikeWorld.x, posSpikeWorld.y, 0);
            spike.transform.localScale = scaleSpikes[i];
            spike.gameObject.name = "Spike "+id;
        }
    }
}
