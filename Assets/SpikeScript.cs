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
    /// Original translation speed of the spikes
    /// </summary>
    private float speed = 0.1f;

    /// <summary>
    /// The original number of seconds before the spike go up
    /// </summary>
    public float secondsBeforeUp = 2.5f;

    /// <summary>
    /// Time when the spike begin to go up
    /// </summary>
    private float timeBeginningUp = 0;

    /// <summary>
    /// Position of the spike when in the ground
    /// </summary>
    private float posWhenIn = 0;

    /// <summary>
    /// The speed of the spike
    /// </summary>
    private float actualSpeed = 0;

    /// <summary>
    /// The number of seconds before the spike go up
    /// </summary>
    private float actualTimeToGoUp = 0;

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;

        float rotation = this.transform.rotation.z;
        Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);
        if (rotation > 0 || rotation < 0)
            posWhenIn = posSpikeCam.x;
        else if (rotation == 0)
            posWhenIn = posSpikeCam.y;

        actualTimeToGoUp = secondsBeforeUp;
        actualSpeed = speed;

        subscribeEvent<int>();
    }

    void Update()
    {
        //We increase the speed and decrease the cooldown
        actualTimeToGoUp = secondsBeforeUp * Mathf.Max(0.1f, 1 - Time.timeSinceLevelLoad / 300);
        actualSpeed = speed * Mathf.Min(5, 1 + Time.timeSinceLevelLoad / 60);

        // We apply translation to the spikes
        // Then we check for the ending position of the spike

        //Go out has priority vs go in
        if (mustBeInMovementOut && Time.time > (timeBeginningUp + actualTimeToGoUp))
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;

            float rotation = this.transform.rotation.z;
            Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);
            Vector3 perfectPosition = new Vector3(this.transform.position.x,
                                                    Camera.main.ViewportToWorldPoint(new Vector3(posSpikeCam.x,
                                                                                                 translateInCamera,
                                                                                                 posSpikeCam.z)).y,
                                                   this.transform.position.z);

            if (rotation > 0 && posSpikeCam.x <= translateInCamera)
            {
                this.transform.Translate(new Vector3(0, -actualSpeed, 0));
                Vector3 newPos = Camera.main.WorldToViewportPoint(this.transform.position);
                if (newPos.x >= translateInCamera)
                {
                    perfectPosition = new Vector3(
                                                    Camera.main.ViewportToWorldPoint(new Vector3(translateInCamera,
                                                                                                 posSpikeCam.y,
                                                                                                 posSpikeCam.z)).x,
                                                    this.transform.position.y,
                                                    this.transform.position.z);
                    this.transform.position = perfectPosition;
                    mustBeInMovementOut = false;
                }
            }
            else if (rotation < 0 && posSpikeCam.x >= (1 - translateInCamera))
            {
                this.transform.Translate(new Vector3(0, -actualSpeed, 0));
                Vector3 newPos = Camera.main.WorldToViewportPoint(this.transform.position);
                if (newPos.x <= (1 - translateInCamera))
                {
                    perfectPosition = new Vector3(
                                                    Camera.main.ViewportToWorldPoint(new Vector3((1 - translateInCamera),
                                                                                                 posSpikeCam.y,
                                                                                                 posSpikeCam.z)).x,
                                                    this.transform.position.y,
                                                    this.transform.position.z);
                    this.transform.position = perfectPosition;
                    mustBeInMovementOut = false;
                }
            }
            else if (rotation == 0 && posSpikeCam.y <= translateInCamera)
            {
                this.transform.Translate(new Vector3(0, actualSpeed, 0));
                Vector3 newPos = Camera.main.WorldToViewportPoint(this.transform.position);
                if (newPos.y >= translateInCamera)
                {
                    this.transform.position = perfectPosition;
                    mustBeInMovementOut = false;
                }
            }
        }
        else if(mustBeInMovementOut)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (mustBeInMovementIn)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;

            float rotation = this.transform.rotation.z;
            Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);
            Vector3 perfectPosition = new Vector3(  this.transform.position.x, 
                                                    Camera.main.ViewportToWorldPoint(new Vector3(posSpikeCam.x, 
                                                                                                 posWhenIn,
                                                                                                 posSpikeCam.z)).y, 
                                                    this.transform.position.z);

            if (rotation > 0 && posSpikeCam.x >= posWhenIn)
            {
                this.transform.Translate(new Vector3(0, actualSpeed, 0));
                Vector3 newPos = Camera.main.WorldToViewportPoint(this.transform.position);
                if (newPos.x <= posWhenIn)
                {
                    perfectPosition = new Vector3(
                                                    Camera.main.ViewportToWorldPoint(new Vector3(posWhenIn,
                                                                                                 posSpikeCam.y,
                                                                                                 posSpikeCam.z)).x,
                                                    this.transform.position.y,
                                                    this.transform.position.z);
                    this.transform.position = perfectPosition;
                    mustBeInMovementIn = false;
                }
            }
            else if (rotation < 0 && posSpikeCam.x <= posWhenIn)
            {
                this.transform.Translate(new Vector3(0, actualSpeed, 0));
                Vector3 newPos = Camera.main.WorldToViewportPoint(this.transform.position);
                if (newPos.x >= posWhenIn)
                {
                    perfectPosition = new Vector3(
                                                    Camera.main.ViewportToWorldPoint(new Vector3(posWhenIn,
                                                                                                 posSpikeCam.y,
                                                                                                 posSpikeCam.z)).x,
                                                    this.transform.position.y,
                                                    this.transform.position.z);
                    this.transform.position = perfectPosition;
                    mustBeInMovementIn = false;
                }
            }
            else if (rotation == 0 && posSpikeCam.y >= posWhenIn)
            {
                this.transform.Translate(new Vector3(0, -actualSpeed, 0));
                Vector3 newPos = Camera.main.WorldToViewportPoint(this.transform.position);
                if (newPos.y <= posWhenIn)
                {
                    this.transform.position = perfectPosition;
                    mustBeInMovementIn = false;
                }
            }
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
            mustBeInMovementOut = true;
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
            mustBeInMovementIn = true;
    }
}
