using UnityEngine;
using System.Collections;

public class MyPhysics : MonoBehaviour
{
    public Vector2 playerGivenAcceleration;
    private Vector2 acceleration;
    private Vector2 speed;
    public Vector2 position { get; private set; }

    private MovePlayer move;

    /// <summary>
    /// The gravity of the world
    /// </summary>
    [SerializeField]
    private float gravity;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float dragCoeff;

	// Use this for initialization
	void Start ()
    {
        speed = new Vector2(0,0);

        move = this.gameObject.GetComponent<MovePlayer>();
	}
	
    private void drag()
    {
        if (speed.x > 0)
        {
            if(playerGivenAcceleration.x<=0)
                speed.x -= dragCoeff;
            if (speed.x < 0)
                speed.x = 0;
            if (speed.x > maxSpeed)
                speed.x = maxSpeed;
        }
        else if (speed.x < 0)
        {
            if(playerGivenAcceleration.x>=0)
                speed.x += dragCoeff;
            if (speed.x > 0)
                speed.x = 0;
            if (speed.x < -maxSpeed)
                speed.x = -maxSpeed;
        }
    }

    /// <summary>
    /// calcul de la nouvelle acceleration en prenant compte de la gravité
    /// </summary>
	private void gravityCalculator()
    {
        acceleration.y -= gravity;
    }
    
    // Update is called once per frame
	void Update ()
    {
        acceleration = playerGivenAcceleration;

        bool isGravityEnabled = false;
        if(move.isInContactWithPlatform)
        {
            if (acceleration.y < 0)
                acceleration.y = 0;
            if (speed.y < 0)
                speed.y = 0;
        }
        else if (move.isInContactWithWall)
        {
            isGravityEnabled = true;
        }
        else//no contact
        {
            isGravityEnabled = true;
        }

        if(isGravityEnabled)
            gravityCalculator();


        //Debug.Log("input :"+playerGivenAcceleration);
        //update position
        speed = speed + (acceleration * Time.deltaTime);
        drag();
        position = position + (speed * Time.deltaTime);
        //Debug.Log("speed :" + speed);
        //Debug.Log("position :" + position);
	}

    public void playerHasCollideWall(Transform wallTranform)
    {
        ;
    }

    public void playerHasCollidePlatform(Transform platTranform)
    {
        ;
    }
}
