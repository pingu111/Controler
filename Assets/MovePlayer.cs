using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{

    public float speedX;
    public float speedJumpY;

    // Is the player in contact wiith a plateform ?
    public bool isInContactWithPlatform;

    // Is the player in contact wiith a wall ?
    public bool isInContactWithWall;

    // Has the player already used his double jump ?
    private bool doubleJumpedUsed = false;

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        controls();
        this.gameObject.transform.position = this.gameObject.GetComponent<MyPhysics>().position;

        Vector3 cameraLimits = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        if (cameraLimits.x < 0 || cameraLimits.x > 1f ||
            cameraLimits.y < 0 || cameraLimits.y > 1f)
        {
            EventManager.raise(MyEventTypes.ONLOSE);
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject + " entered");

        if (collision.gameObject.tag == "Spike")
        {
            if(collision.gameObject.GetComponent<SpikeScript>() != null && collision.gameObject.GetComponent<SpikeScript>().canKillPlayer)
                EventManager.raise(MyEventTypes.ONLOSE);
        }
        else
        {
            if (collision.gameObject.tag == "Platform")
            {
                if (collision.gameObject.GetComponent<PlatformScript>() != null)
                    collision.gameObject.GetComponent<PlatformScript>().platformTouched();

                isInContactWithPlatform = true;
                doubleJumpedUsed = false;
            }
            else if (collision.gameObject.tag == "RightWallPlatform"
                || collision.gameObject.tag == "LeftWallPlatform")
            {
                isInContactWithWall = true;
                doubleJumpedUsed = false;
            }

            if (collision.GetType() == typeof(BoxCollider))
                this.GetComponent<MyPhysics>().playerHasCollided((BoxCollider)collision);
            else
                Debug.Log("CRITICAL PLATFORM NOT BOXCOLLIDER");
        }
    }

    void OnTriggerExit(Collider collision)
    {
        Debug.Log(collision.gameObject + " left");

        if (collision.gameObject.tag == "Platform")
                isInContactWithPlatform = false;
        else if (collision.gameObject.tag == "RightWallPlatform"
            || collision.gameObject.tag == "LeftWallPlatform")
                isInContactWithWall = false;
        this.GetComponent<MyPhysics>().playerHasExitCollider((BoxCollider)collision);
    }

    /// <summary>
    /// Get the inputs of the player, and apply it to the player
    /// </summary>
    void controls()
    {
        float xAxis = Input.GetAxis("XAxis");
        if (!isInContactWithWall)
            this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(xAxis * speedX, this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration.y, 0);

        if (Input.GetButtonDown("Jump") && (isInContactWithPlatform || !doubleJumpedUsed))
        {
            this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration.x, speedJumpY, 0);
            if (!isInContactWithPlatform && !doubleJumpedUsed)
                doubleJumpedUsed = true;
        }
        else
        {
            GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(GetComponent<MyPhysics>().playerGivenAcceleration.x, 0, 0);
        }
    }
}








