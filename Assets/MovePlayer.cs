using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    /// <summary>
    /// The multiplicator of the speed 
    /// </summary>
    public float speedX;

    /// <summary>
    /// The height of the jump
    /// </summary>
    public float speedJumpY;

    // Is the player in contact wiith a plateform ?
    public bool isInContactWithPlatform;

    // Is the player in contact wiith a wall ?
    public bool isInContactWithWall;

    // Has the player already used his double jump ?
    private bool doubleJumpedUsed = false;

    // Lats platform collided
    private BoxCollider lastCollider;

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        controls();
        movePlayerFromPhysics();
    }

    /// <summary>
    /// We use a lateUpdate because of the collisions 
    /// </summary>
    void LateUpdate()
    {
        movePlayerFromPhysics();
    }

    private void movePlayerFromPhysics()
    {
        this.gameObject.transform.position = this.gameObject.GetComponent<MyPhysics>().position;

        Vector3 cameraLimits = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        if (cameraLimits.x < 0 || cameraLimits.x > 1f ||
            cameraLimits.y < 0 || cameraLimits.y > 1f)
        {
            Debug.Log("Lose hors ecran");

            EventManager.raise(MyEventTypes.ONLOSE);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject + " entered");

        if (collision.gameObject.tag == "Spike")
        {
            if(collision.gameObject.GetComponent<SpikeScript>() != null && collision.gameObject.GetComponent<SpikeScript>().canKillPlayer)
            {
                Debug.Log("Lose " + collision.gameObject.GetComponent<SpikeScript>().canKillPlayer);
                EventManager.raise(MyEventTypes.ONLOSE);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Platform")
            {
                // If it's a platform and not the ground
                if (collision.gameObject.GetComponent<PlatformScript>() != null)
                {
                    if (collision.GetType() == typeof(BoxCollider))
                        lastCollider = (BoxCollider)collision;
                    collision.gameObject.GetComponent<PlatformScript>().platformTouched();
                    EventManager.addActionToEvent(MyEventTypes.PLATFORMHIDEN, platformExited);
                }

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
                Debug.Log("CRITICAL " + collision.gameObject + " PLATFORM NOT BOXCOLLIDER");
        }
    }

    /// <summary>
    /// Called when the platform disapear
    /// </summary>
    void platformExited()
    {
        this.GetComponent<MyPhysics>().playerHasExitCollider((BoxCollider)lastCollider);
    }

    void OnTriggerExit(Collider collision)
    {
        Debug.Log(collision.gameObject + " exited");

        // If we exited normally the platform
        if (lastCollider == collision)
            EventManager.removeActionFromEvent(MyEventTypes.PLATFORMHIDEN, platformExited);

        if (collision.gameObject.tag == "Platform")
        {
            isInContactWithPlatform = false;
            if (collision.GetType() == typeof(BoxCollider))
                this.GetComponent<MyPhysics>().playerHasExitCollider((BoxCollider)collision);
            else
                Debug.Log("CRITICAL " + collision.gameObject + " PLATFORM NOT BOXCOLLIDER");
        }
        else if (collision.gameObject.tag == "RightWallPlatform"
            || collision.gameObject.tag == "LeftWallPlatform")
        {
            isInContactWithWall = false;
            if (collision.GetType() == typeof(BoxCollider))
                this.GetComponent<MyPhysics>().playerHasExitCollider((BoxCollider)collision);
            else
                Debug.Log("CRITICAL " + collision.gameObject + " PLATFORM NOT BOXCOLLIDER");
        }
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








