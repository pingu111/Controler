using UnityEngine;
using System.Collections;

public class MyPhysics : MonoBehaviour
{
    /// <summary>
    /// acceleration donnee par le joueur
    /// </summary>
    public Vector2 playerGivenAcceleration;
    /// <summary>
    /// acceleration reelle
    /// </summary>
    private Vector2 acceleration;
    /// <summary>
    /// la vitesse effective du joueur
    /// </summary>
    private Vector2 speed;
    /// <summary>
    /// la position calculee du joueur
    /// </summary>
    public Vector2 position { get; private set; }

    /// <summary>
    /// le joueur sur qui le gravite joue
    /// </summary>
    private MovePlayer move;
    private bool isInContactWithLeftWall;
    private bool isInContactWithRightWall;
    private bool isInContactWithPlateform;
    private bool isInContactWithRoof;


    /// <summary>
    /// The gravity of the world
    /// </summary>
    [SerializeField]
    private float gravity;

    /// <summary>
    /// la vitesse max du joueur
    /// </summary>
    [SerializeField]
    private float maxSpeed;

    /// <summary>
    /// coefficient de frottement au sol
    /// </summary>
    [SerializeField]
    private float groundDragCoeff;

    /// <summary>
    /// coefficient de frottement en l'air
    /// </summary>
    [SerializeField]
    private float airDragCoeff;

	// Use this for initialization
	void Start ()
    {
        speed = new Vector2(0,0);
        isInContactWithLeftWall = false;
        isInContactWithRightWall = false;
        isInContactWithPlateform = false;
        isInContactWithRoof = false;
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
        float coeff = groundDragCoeff;
        bool isGravityEnabled = true;

        if (isInContactWithLeftWall || isInContactWithPlateform || isInContactWithRightWall || isInContactWithRoof)
        {
            if (isInContactWithRoof)
            {
                coeff = airDragCoeff;
                if (acceleration.y > 0)
                    acceleration.y = 0;
                if (speed.y > 0)
                    speed.y = 0;
            }
            else if (isInContactWithPlateform)
            {
                isGravityEnabled = false;
                if (acceleration.y < 0)
                    acceleration.y = 0;
                if (speed.y < 0)
                    speed.y = 0;
            }
            if (isInContactWithLeftWall)
            {
                if (acceleration.x < 0)
                    acceleration.x = 0;
                if (speed.x < 0)
                    speed.x = 0;
            }
            else if (isInContactWithRightWall)
            {
                if (acceleration.x > 0)
                    acceleration.x = 0;
                if (speed.x > 0)
                    speed.x = 0;
            }
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

    public void playerHasCollided(BoxCollider collider)
    {
        //wall collision

        //TODO gerer le nouveau boolean
        float distanceX = ((wallTranform.bounds.size.x + move.GetComponent<Collider>().bounds.size.x) / 2);
        float wallPosition = wallTranform.transform.position.x + wallTranform.center.x;

        if (speed.x < 0)//on collide le mur sur sa droite
        {
            speed.x = 0;
            position = new Vector2(wallPosition + distanceX, position.y);
        }
        else if (speed.x > 0)//on collide le mur sur sa gauche
        {
            speed.x = 0;
            position = new Vector2(wallPosition - distanceX, position.y);
        }
        //si la speed est nul on a pas bouge

     

        //plateform collision


        //TODO gerer le nouveau bool
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
           // speed.y = 0;
            position = new Vector2(position.x, platformPosition - distanceY);
        }
        Debug.Log(position);
        //si la speed est nul on a pas bouge

    }
}
