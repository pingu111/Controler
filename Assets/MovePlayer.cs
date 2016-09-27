﻿using UnityEngine;
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
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject + " entered");
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "WallJumpPlatform")
        {
            isInContactWithPlatform = true;
            doubleJumpedUsed = false;
        }
        else if (collision.gameObject.tag == "WallJumpPlatform")
        {
            isInContactWithWall = true;
            doubleJumpedUsed = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject + " left");
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
            this.gameObject.GetComponent<MyPhysics>().acceleration = new Vector3(xAxis * speedX, this.gameObject.GetComponent<MyPhysics>().acceleration.y, 0);

        if (Input.GetButtonDown("Jump") && (isInContactWithPlatform || !doubleJumpedUsed))
        {
            this.gameObject.GetComponent<MyPhysics>().acceleration = new Vector3(this.gameObject.GetComponent<MyPhysics>().acceleration.x, speedJumpY, 0);
            if (!isInContactWithPlatform && !doubleJumpedUsed)
                doubleJumpedUsed = true;
        }
    }
}
