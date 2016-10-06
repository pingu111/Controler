using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {

    /// <summary>
    /// Number of seconds before respawn of the platform
    /// </summary>
    private float cooldownRespawn;

    /// <summary>
    /// Last time.time when the platform was desactivated
    /// </summary>
    private float lastTimeDesactivated;

    /// <summary>
    /// Number of seconds before the desapear of the platform
    /// </summary>
    private float timeBeforeDisapear;

	// Use this for initialization
	void Start ()
    {
        lastTimeDesactivated = 0;
        cooldownRespawn = 5;
        timeBeforeDisapear = 1;
        this.transform.parent.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 10);
    }

    void Update()
    {
        
        if (lastTimeDesactivated != 0)
        {
            if (Time.time - lastTimeDesactivated > timeBeforeDisapear + cooldownRespawn)
            {
                respawnPlatform();

            }
            else if (Time.time - lastTimeDesactivated > timeBeforeDisapear)
            {
                hidePlatform();
            }
        }
    }
	
    public void platformTouched()
    {
        lastTimeDesactivated = Time.time;
        this.transform.parent.gameObject.GetComponent<Renderer>().material.color = new Color(100,0,0);
    }

    /// <summary>
    /// Hide the platform
    /// </summary>
    public void hidePlatform()
    {
        if(this.gameObject.GetComponent<BoxCollider>().enabled)
        {
            EventManager.raise(MyEventTypes.PLATFORMHIDEN);
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.transform.parent.GetComponent<Renderer>().enabled = false;
        }
    }

    /// <summary>
    /// Print the platform, and enable the collider
    /// </summary>
    public void respawnPlatform()
    {
        lastTimeDesactivated = 0;
        this.transform.parent.GetComponent<Renderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider>().enabled = true;

        this.transform.parent.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 10);
    }
}
