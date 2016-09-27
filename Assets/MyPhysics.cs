using UnityEngine;
using System.Collections;

public class MyPhysics : MonoBehaviour
{

    public Vector2 acceleration;
    private Vector2 speed;
    public Vector2 position { get; private set; }
    private MovePlayer move;

    [SerializeField]
    float gravity;
    [SerializeField]
    float airFriction;
    [SerializeField]
    float groundFriction;

	// Use this for initialization
	void Start ()
    {
        acceleration = new Vector2(0,0);
        speed = new Vector2(0,0);

        move = this.gameObject.GetComponent<MovePlayer>();
	}

    /**
        reduit les deux valeurs de values de value correspondant vers 0 (s'arrete a 0 si on le dépasse)
    */
    private Vector2 drag(Vector2 value, Vector2 values)
    {
        Vector2 toReturn = values;
        //change x value
        if (values.x > 0)
        {
            toReturn.x = toReturn.x - value.x;
            if (toReturn.x < 0)
                toReturn.x = 0;
        }
        else if(values.x < 0)
        {
            toReturn.x = toReturn.x + value.x;
            if (toReturn.x > 0)
                toReturn.x = 0;
        }
            
        //change y value
        if (values.y > 0)
        {
            toReturn.y = toReturn.y - value.y;
            if (toReturn.y < 0)
                toReturn.y = 0;
        }
        else if (values.y < 0)
        {
            toReturn.y = toReturn.y + value.y;
            if (toReturn.y > 0)
                toReturn.y = 0;
        }
            

        return toReturn;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //changements pour la friction
        Vector2 friction = new Vector2(0,0);
        if(move.isInContactWithPlatform)
        {
            friction = speed*groundFriction;
        }
        else
        {
            //gestion de la gravite
            acceleration.y -= gravity;

            friction = speed*airFriction;
        }
        acceleration = drag(friction, acceleration);

        

        //update speed et position
        speed = speed + (acceleration * Time.deltaTime);
        position = position + (speed * Time.deltaTime);
	}
}
