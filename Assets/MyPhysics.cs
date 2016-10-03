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

    public void playerHasCollideWall(BoxCollider wallTranform)
    {
        float distanceX = ((wallTranform.bounds.size.x + move.GetComponent<Collider>().bounds.size.x) / 2);
        float wallPosition = wallTranform.transform.position.x + wallTranform.center.x;

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

    public void playerHasCollidePlatform(BoxCollider platTranform)
    {
        //distance entre le collider et le joueur
        float distanceY = ((platTranform.bounds.size.y + move.GetComponent<Collider>().bounds.size.y) / 2);
        Debug.Log(distanceY);
        //la position du collider par rapport a l'object
        float center = platTranform.center.y;
        Debug.Log(center);
        //la position de l'object portant le collider
        float positionCollider = platTranform.transform.position.y;
        Debug.Log(positionCollider);
        //position du collider
        float platformPosition = center + positionCollider;
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
