using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour
{
    /// <summary>
    /// ID of the spike
    /// </summary>
    public int idSpike = 0;

    /// <summary>
    /// ID of the group of spikes to raise
    /// </summary>
    public int idGroupSpike = 0;

    /// <summary>
    /// Must be the spike in mouvement to out ?
    /// </summary>
    private bool mustBeInMovementOut = false;

    /// <summary>
    /// Must be the spike in mouvement to in ?
    /// </summary>
    private bool mustBeInMovementIn = false;

    /// <summary>
    /// % of the camera that can see the spike, when out
    /// </summary>
    private float translateInCamera = 0.05f;

     /// <summary>
    /// translation speed of the spikes
    /// </summary>
    private float speed = 0.1f;

    /// <summary>
    /// The number of seconds before the spike go up
    /// </summary>
    public float secondsBeforeUp = 2.5f;

    /// <summary>
    /// Time when the spike begin to go up
    /// </summary>
    private float timeBeginningUp = 0;

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;

        subscribeEvent<int>();
    }

    void Update()
    {
        if (mustBeInMovementOut && Time.time > (timeBeginningUp + secondsBeforeUp))
        {
            //Debug.Log(Time.time + " " + timeBeginningUp + " " + secondsBeforeUp);

            this.gameObject.GetComponent<Renderer>().material.color = Color.red;

            float rotation = this.transform.rotation.z;
            Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);
            if (rotation > 0 && posSpikeCam.x < translateInCamera)
                this.transform.Translate(new Vector3(0, -speed, 0));
            else if (rotation < 0 && posSpikeCam.x > (1 - translateInCamera))
                this.transform.Translate(new Vector3(0, -speed, 0));
            else if (rotation == 0 && posSpikeCam.y < translateInCamera)
                this.transform.Translate(new Vector3(0, speed, 0));
        }
        else if (mustBeInMovementIn)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;

            float rotation = this.transform.rotation.z;
            Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);

            if (rotation == 90 && posSpikeCam.x > -0.1f)
                this.transform.Translate(new Vector3(0, speed, 0));
            else if (rotation == -90 && posSpikeCam.x < 1.1f)
                this.transform.Translate(new Vector3(0, speed, 0));
            else if (rotation == 0 && posSpikeCam.y > -0.1f)
                this.transform.Translate(new Vector3(0, -speed, 0));
        }
    }

    private void subscribeEvent<T>()
    {
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEOUT, raiseSpike);
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEIN, retractSpike);
        EventManager.addActionToEvent<int>(MyEventTypes.GROUPSPIKEOUT, raiseGroupSpike);
        EventManager.addActionToEvent<int>(MyEventTypes.GROUPSPIKEIN, retractGroupSpike);
    }

    public void raiseSpike(int i)
    {
        if (i == idSpike)
        {
            raiseSpike();
        }
    }

    public void raiseGroupSpike(int i)
    {
        if (i == idGroupSpike)
        {
            raiseSpike();
        }
    }

    private void raiseSpike()
    {
        timeBeginningUp = Time.time;

        this.gameObject.GetComponent<Renderer>().material.color = Color.red;

        mustBeInMovementOut = true;
        mustBeInMovementIn = false;

    }

    public void retractSpike(int i)
    {
        if (i == idSpike)
            retractThisSpike();
    }

    public void retractGroupSpike(int i)
    {
        if (i == idGroupSpike)
            retractThisSpike();
    }

    private void retractThisSpike()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        mustBeInMovementIn = true;
        mustBeInMovementOut = false;
    }

}
