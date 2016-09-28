using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    public float speedX;
    public float speedJumpY;

    // Is the player in contact wiith a plateform ?
    public bool isInContactWithPlatform;

    // Is the player in contact wiith a wall ?
    public bool isInContactWithWall;

    // Has the player already used his double jump ?
    private bool doubleJumpedUsed = false;

	// Use this for initialization
	void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
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

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject + " entered");
        if (collision.gameObject.tag == "Platform")
        {
            isInContactWithPlatform = true;
            doubleJumpedUsed = false;
            if (collision.collider.GetType() == typeof(BoxCollider))
                this.GetComponent<MyPhysics>().playerHasCollidePlatform((BoxCollider)collision.collider);
            else
                Debug.Log("CRITICAL PLATFORM NOT BOXCOLLIDER");
        }
        else if (collision.gameObject.tag == "WallJumpPlatform")
        {
            isInContactWithWall = true;
            doubleJumpedUsed = false;
            if (collision.collider.GetType() == typeof(BoxCollider))
                this.GetComponent<MyPhysics>().playerHasCollideWall((BoxCollider)collision.collider);
            else
                Debug.Log("CRITICAL PLATFORM NOT BOXCOLLIDER");
        }
        else if(collision.gameObject.tag == "Spike")
        {
            EventManager.raise(MyEventTypes.ONLOSE);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        //Debug.Log(collision.gameObject + " left");
        if (collision.gameObject.tag == "Platform")
            isInContactWithPlatform = false;
        else if (collision.gameObject.tag == "WallJumpPlatform")
            isInContactWithWall = false;
    }

    /// <summary>
    /// Get the inputs of the player, and apply it to the player
    /// </summary>
    void controls()
    {
        float xAxis = Input.GetAxis("XAxis");
        if(!isInContactWithWall)
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
