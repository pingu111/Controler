using UnityEngine;
using System.Collections;

public class MyPhysics : MonoBehaviour
{

    private Vector2 acceleration;
    public Vector2 playerGivenAcceleration;
    private Vector2 speed;
    public Vector2 position { get; private set; }
    private MovePlayer move;

    /// <summary>
    /// The gravity of the world
    /// </summary>
    [SerializeField]
    float gravity;

    /// <summary>
    /// The friction of the air in this world
    /// </summary>
    [SerializeField]
    float airFriction;

    /// <summary>
    /// The friction of the ground on this world
    /// </summary>
    [SerializeField]
    float groundFriction;

    /// <summary>
    /// the mass of the object
    /// </summary>
    [SerializeField]
    float mass;

	// Use this for initialization
	void Start ()
    {
        acceleration = new Vector2(0,0);
        speed = new Vector2(0,0);

        move = this.gameObject.GetComponent<MovePlayer>();
	}




    /// <summary>
    /// reduit l'acceleration de value vers 0 (s'arrete a 0 si on le dépasse)
    /// </summary>
    /// <param name="value">la valeur du drag. réduction de l'acceleration</param>
    private void drag(Vector2 value)
    {
        //change x value
        if (acceleration.x > 0)
        {
            acceleration.x = acceleration.x - value.x;
            if (acceleration.x < 0)
            {
                acceleration.x = 0;
                speed.x = 0;
            }
        }
        else if(acceleration.x < 0)
        {
            acceleration.x = acceleration.x + value.x;
            if (acceleration.x > 0)
            {
                acceleration.x = 0;
                speed.x = 0;
            }
        }
            
        //change y value
        if (acceleration.y > 0)
        {
            acceleration.y = acceleration.y - value.y;
            if (acceleration.y < 0)
            {
                acceleration.y = 0;
                speed.y = 0;
            }
        }
        else if (acceleration.y < 0)
        {
            acceleration.y = acceleration.y + value.y;
            if (acceleration.y > 0)
            {
                acceleration.y = 0;
                speed.y = 0;
            }
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

        bool isGravityAnabled = false;
        //changements pour la friction
        Vector2 friction = new Vector2(0,0);
        if(move.isInContactWithPlatform)
        {
            if (acceleration.y < 0)
                acceleration.y = 0;
            if (speed.y < 0)
                speed.y = 0;
            friction = new Vector2(groundFriction,0);
        }
        else if (move.isInContactWithWall)
        {
            friction = new Vector2(groundFriction,0);
            isGravityAnabled = true;
        }
        else
        {
            isGravityAnabled = true;
            friction = new Vector2(airFriction, airFriction);
        }
        drag(friction);

        if(isGravityAnabled)
            gravityCalculator();

        Debug.Log("input :"+playerGivenAcceleration);
        Debug.Log("calculus :"+acceleration);
        //update speed et position
        speed = speed + (acceleration * Time.deltaTime);
        Debug.Log("speed :"+speed);
        position = position + (speed * Time.deltaTime);
        Debug.Log("position :" + position);
	}
}
