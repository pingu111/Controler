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
    private float groundDragCoeff;

    [SerializeField]
    private float airDragCoeff;

	// Use this for initialization
	void Start ()
    {
        speed = new Vector2(0,0);

        move = this.gameObject.GetComponent<MovePlayer>();
	}
	
    private void drag(float coeff)
    {
        if (speed.x > 0)
        {
            if(playerGivenAcceleration.x<=0)
                speed.x -= coeff;
            if (speed.x < 0)
                speed.x = 0;
            if (speed.x > maxSpeed)
                speed.x = maxSpeed;
        }
        else if (speed.x < 0)
        {
            if(playerGivenAcceleration.x>=0)
                speed.x += coeff;
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
        //le coefficient de friction correspondant pour l'etat du player
        float coeff = 0;
        bool isGravityEnabled = false;
        if(move.isInContactWithPlatform)
        {
            coeff = groundDragCoeff;
            if (acceleration.y < 0)
                acceleration.y = 0;
            if (speed.y < 0)
                speed.y = 0;
        }
        else if (move.isInContactWithWall)
        {
            coeff = groundDragCoeff;
            isGravityEnabled = true;
        }
        else//no contact
        {
            coeff = airDragCoeff;
            isGravityEnabled = true;
        }
        Debug.Assert(coeff != 0);
        if(isGravityEnabled)
            gravityCalculator();


        //Debug.Log("input :"+playerGivenAcceleration);
        //update position
        speed = speed + (acceleration * Time.deltaTime);
        drag(coeff);
        position = position + (speed * Time.deltaTime);
        //Debug.Log("speed :" + speed);
        //Debug.Log("position :" + position);
	}

    public void playerHasCollideWall(Transform wallTranform)
    {
        float distanceX = ((wallTranform.GetComponent<Collider>().bounds.size.x + move.GetComponent<Collider>().bounds.size.x) / 2);
        float wallPosition = wallTranform.parent.position.x + wallTranform.position.x + wallTranform.GetComponent<BoxCollider>().center.x;

        if (speed.x<0)//on collide le mur sur sa droite
        {
            speed.x = 0;
            position = new Vector2(wallPosition + distanceX,position.y);
        }
        else if (speed.x>0)//on collide le mur sur sa gauche
        {
            speed.x = 0;
            position = new Vector2(wallPosition - distanceX, position.y);
        }
        //si la speed est nul on a pas bouge
    }

    public void playerHasCollidePlatform(Transform platTranform)
    {
        float distanceY = ((platTranform.GetComponent<Collider>().bounds.size.y + move.GetComponent<Collider>().bounds.size.y) / 2);
        Debug.Log(distanceY);
        float center = platTranform.GetComponent<BoxCollider>().center.y;
        Debug.Log(center);
        float positionPl = platTranform.parent.position.y + platTranform.position.y;
        Debug.Log(positionPl);
        float platformPosition = center + positionPl;
        Debug.Log(platformPosition);

        if (speed.y < 0)//on collide le mur sur le haut
        {
            speed.y = 0;
            position = new Vector2(position.x, platformPosition + distanceY);
        }
        else if (speed.y > 0)//on collide le mur sur le bas
        {
            speed.y = 0;
            position = new Vector2(position.x, platformPosition - distanceY);
        }
        Debug.Log(position);
        //si la speed est nul on a pas bouge

    }
}
