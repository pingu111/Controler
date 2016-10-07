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
    private List<Vector3> posSpike;

    /// <summary>
    /// List of the scale of the spikes
    /// Scale : whatever you want
    /// </summary>
    private List<Vector3> scaleSpikes;

    /// <summary>
    /// List of the orientations of the spikes
     /// </summary>
    private List<Vector3> orientationsSpikes;

    /// <summary>
    /// List of the groups of spikes
    /// </summary>
    private List<int> idGroupSpikes;

    /// <summary>
    /// The number of spikes on the ground
    /// </summary>
    public int nbSpikesGround = 1;

    /// <summary>
    /// The number of spikes on the walls
    /// </summary>
    public int nbSpikesWall = 1;

    /// <summary>
    /// The number of group of spikes on the ground
    /// </summary>
    public int nbGroupGround = 4;

    /// <summary>
    /// The number of group of spikes on the wall
    /// </summary>
    public int nbGroupWall = 4;

    /// <summary>
    /// Working resolution
    /// </summary>
    public Vector2 normalResolution;

    // Use this for initialization
    void Start ()
    {
        Debug.Assert(nbSpikesGround % nbGroupGround == 0);
        Debug.Assert(nbSpikesWall % nbGroupWall == 0);

        // XML read of the spikes ?

        posSpike = new List<Vector3>();
        scaleSpikes = new List<Vector3>();
        orientationsSpikes = new List<Vector3>();
        idGroupSpikes = new List<int>();

        // We calculate the position of the wall spikes
        float scaleY = 0;
        float ratioSpikeInScreen = ((float)Screen.width / (float)Screen.height);
        Debug.Log(ratioSpikeInScreen+" "+ Screen.width+" "+ Screen.height);
        // Sur un echantillon representatif
        scaleY = 2.0f*ratioSpikeInScreen + 0.4f;
        Debug.Log(scaleY);

        for (int i = 0; i < nbSpikesGround; i++)
        {
            float delta =(float)( 1.0f / (float)(nbSpikesGround + 1));
            float posX = delta + i * delta;
            posSpike.Add(new Vector3(posX, -0.185f, 0));

            scaleSpikes.Add(new Vector3(0.5f, 4, 1));
            orientationsSpikes.Add(new Vector3(0,0,0));

            int idGroup = (int)(i / (nbSpikesGround / nbGroupGround)) + 1;
            idGroupSpikes.Add(idGroup);
        }

        for (int i = 0; i < nbSpikesWall; ++i)
        {
            float delta = (float)(1.0f / (float)(nbSpikesWall + 1));
            float posY = delta + i * delta;
            posSpike.Add(new Vector3(-0.105f, posY, 0));

            scaleSpikes.Add(new Vector3(0.5f, scaleY, 1));
            orientationsSpikes.Add(new Vector3(0, 0, 90));

            int idGroup = (int)(i / (nbSpikesWall / nbGroupWall)) + 1 + nbGroupGround;
            idGroupSpikes.Add(idGroup);
        }

        for (int i = 0; i < nbSpikesWall; i++)
        {
            float delta = (float)(1.0f / (float)(nbSpikesWall + 1));
            float posY = delta + i * delta;
            posSpike.Add(new Vector3(1.105f, posY, 0));

            scaleSpikes.Add(new Vector3(0.5f, scaleY, 1));
            orientationsSpikes.Add(new Vector3(0, 0, -90));

            int idGroup = (int)(i / (nbSpikesWall / nbGroupWall)) + 1 + nbGroupGround + nbGroupWall;
            idGroupSpikes.Add(idGroup);
        }
        initSpikes();
	}
	
    /// <summary>
    /// Initialize the walls, ground, and roof
    /// </summary>
    void initSpikes()
    {
        //We create all the spikes
        for (int i = 0; i < posSpike.Count; i ++)
        {
            GameObject spike = Instantiate(spikePrefab);
            int id = i + 1;
            spike.GetComponent<SpikeScript>().idSpike = id;
            spike.GetComponent<SpikeScript>().idGroupSpike = idGroupSpikes[i];
            spike.transform.parent = this.transform;

            Vector3 posSpikeWorld = Camera.main.ViewportToWorldPoint(posSpike[i]);

            if (orientationsSpikes[i].z != 0)
            {
            }

            spike.transform.position = new Vector3(posSpikeWorld.x, posSpikeWorld.y, -1);

            spike.transform.Rotate(orientationsSpikes[i]);
            spike.transform.localScale = scaleSpikes[i];

            spike.GetComponent<SpikeScript>().translateInCamera = 0.05f;

            spike.gameObject.name = "Spike "+id;
        }
    }
}
