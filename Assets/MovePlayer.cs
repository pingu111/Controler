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

    /// <summary>
    /// Single jump
    /// </summary>
    private bool singleJumpUsed = false;


    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInContactWithPlatform || isInContactWithWall)
        {
            doubleJumpedUsed = false;
            singleJumpUsed = false;
        }
        controls();
        movePlayerFromPhysics();
    }

    private void movePlayerFromPhysics()
    {
        this.gameObject.transform.position = this.gameObject.GetComponent<MyPhysics>().position;
        this.gameObject.transform.localScale = this.gameObject.GetComponent<MyPhysics>().scale;

        Vector3 cameraLimits = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        if (cameraLimits.x < 0 || cameraLimits.x > 1f ||
            cameraLimits.y < 0 || cameraLimits.y > 1f)
        {
            EventManager.raise(MyEventTypes.ONLOSE);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == StringEnum.GetStringValue(Tags.SPIKE))
        {
            if(collision.gameObject.GetComponent<SpikeScript>() != null && collision.gameObject.GetComponent<SpikeScript>().canKillPlayer)
            {
                EventManager.raise(MyEventTypes.ONLOSE);
            }
        }
        else
        {
            if (collision.gameObject.tag == StringEnum.GetStringValue(Tags.PLATFORM))
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
                singleJumpUsed = false;
            }
            else if (collision.gameObject.tag == StringEnum.GetStringValue(Tags.RIGHT_WALL)
                || collision.gameObject.tag == StringEnum.GetStringValue(Tags.LEFT_WALL))
            {
                isInContactWithWall = true;
                doubleJumpedUsed = false;
                singleJumpUsed = false;
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
        isInContactWithPlatform = false;
        if(this.GetComponent<MyPhysics>() != null)
            this.GetComponent<MyPhysics>().playerHasExitCollider((BoxCollider)lastCollider);
    }

    void OnTriggerExit(Collider collision)
    {
        // If we exited normally the platform
        if (lastCollider == collision)
            EventManager.removeActionFromEvent(MyEventTypes.PLATFORMHIDEN, platformExited);

        if (collision.gameObject.tag == StringEnum.GetStringValue(Tags.PLATFORM))
        {
            isInContactWithPlatform = false;
        }
        else if (collision.gameObject.tag == StringEnum.GetStringValue(Tags.RIGHT_WALL)
            || collision.gameObject.tag == StringEnum.GetStringValue(Tags.LEFT_WALL))
        {
            isInContactWithWall = false;
        }

        if (collision.gameObject.tag != StringEnum.GetStringValue(Tags.SPIKE))
        {
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
        this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(xAxis * speedX, this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration.y, 0);
        if (this.gameObject.GetComponent<MyPhysics>() != null)
            if (Input.GetButtonDown("Jump"))
            {
                if(!singleJumpUsed)
                {
                    this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration.x, speedJumpY, 0);
                    singleJumpUsed = true;
                    if(this.gameObject.GetComponent<AudioSource>() != null)
                    {
                        AudioSource audio = this.gameObject.GetComponent<AudioSource>();
                        audio.Play();
                    }
                   
                }
                else if (!doubleJumpedUsed && singleJumpUsed)
                {
                    this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(this.gameObject.GetComponent<MyPhysics>().playerGivenAcceleration.x, speedJumpY, 0);
                    doubleJumpedUsed = true;
                    if (this.gameObject.GetComponent<AudioSource>() != null)
                    {
                        AudioSource audio = this.gameObject.GetComponent<AudioSource>();
                        audio.Play();
                    }
                }
            }
            else
            {
                GetComponent<MyPhysics>().playerGivenAcceleration = new Vector3(GetComponent<MyPhysics>().playerGivenAcceleration.x, 0, 0);
            }
    }

    void OnDestroy()
    {
        EventManager.removeActionFromEvent(MyEventTypes.PLATFORMHIDEN, platformExited);
    }
}








