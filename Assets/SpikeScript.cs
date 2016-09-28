using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour
{
    /// <summary>
    /// ID of the spike
    /// </summary>
    public int idSpike = 0;

    /// <summary>
    /// Must be the spike in mouvement to out ?
    /// </summary>
    private bool mustBeInMovementOut = true;

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

    // Use this for initialization
    void Start ()
    {
        subscribeEvent<int>();
        //EventManager.raise<int>(MyEventTypes.SPIKEOUT, 10);
    }

    void Update()
    {
        if (mustBeInMovementOut)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;

            float rotation = this.transform.rotation.z;
            Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);

            if (rotation == 90 && posSpikeCam.x < translateInCamera)
                this.transform.Translate(new Vector3(speed, 0, 0));
            else if (rotation == -90 && posSpikeCam.x > (1 - translateInCamera))
                this.transform.Translate(new Vector3(-speed, 0, 0));
            else if (rotation == 0 && posSpikeCam.y < translateInCamera)
                this.transform.Translate(new Vector3(0, speed, 0));

        }
        else if (mustBeInMovementIn)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;

            float rotation = this.transform.rotation.z;
            Vector3 posSpikeCam = Camera.main.WorldToViewportPoint(this.transform.position);

            if (rotation == 90 && posSpikeCam.x > -0.1f)
                this.transform.Translate(new Vector3(-speed, 0, 0));
            else if (rotation == -90 && posSpikeCam.x < 1.1f)
                this.transform.Translate(new Vector3(speed, 0, 0));
            else if (rotation == 0 && posSpikeCam.y > -0.1f)
                this.transform.Translate(new Vector3(0, -speed, 0));
        }
    }

    private void subscribeEvent<T>()
    {
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEOUT, raiseSpike);
        EventManager.addActionToEvent<int>(MyEventTypes.SPIKEIN, retractSpike);
    }

    public void raiseSpike(int i)
    {
        Debug.Log(i);
        if (i == idSpike)
        {
            raiseThisSpike();
        }
    }

    private IEnumerator raiseThisSpike()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log(this.gameObject.GetComponent<Renderer>().material.color);

        yield return new WaitForSeconds(10);
        mustBeInMovementOut = true;
    }

    public void retractSpike(int i)
    {
        if (i == idSpike)
            retractThisSpike();
    }

    private void retractThisSpike()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        mustBeInMovementIn = true;
    }

}
