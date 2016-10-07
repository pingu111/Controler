using System.Collections.Generic;
using UnityEngine;

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
    /// la scale du joueur calculee
    /// </summary>
    public Vector3 scale { get; private set; }

    private static List<BoxCollider> colliderList;

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
    private float gravity=9;

    /// <summary>
    /// la vitesse max du joueur
    /// </summary>
    [SerializeField]
    private float maxSpeed=10;

    /// <summary>
    /// coefficient de frottement au sol
    /// </summary>
    [SerializeField]
    private float groundDragCoeff=10;

    /// <summary>
    /// coefficient de frottement en l'air
    /// </summary>
    [SerializeField]
    private float airDragCoeff=1;

	// Use this for initialization
	void Start ()
    {
        speed = new Vector2(0,0);
        scale = new Vector3(1, 1, 1);
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

    /// <summary>
    /// check if the player is out of the given walls
    /// reset his position accordingly
    /// </summary>
    // potential problem if distance between wall are smaller than the player
    private void checkOutOfWall()
    {
        Debug.Assert(colliderList != null);
        foreach(BoxCollider collider in colliderList)
        {
            if (position.x<collider.transform.position.x && collider.CompareTag(StringEnum.GetStringValue(Tags.LEFT_WALL)))
            {
                playerHasCollided(collider);
            }
            else if (position.x > collider.transform.position.x && collider.CompareTag(StringEnum.GetStringValue(Tags.RIGHT_WALL)))
            {
                playerHasCollided(collider);
            }
            else if (position.y < collider.transform.position.y && collider.CompareTag(StringEnum.GetStringValue(Tags.PLATFORM)))
            {
                playerHasCollided(collider);
            }
            else if (position.y > collider.transform.position.y && collider.CompareTag(StringEnum.GetStringValue(Tags.ROOF)))
            {
                playerHasCollided(collider);
            }
        }
    }

    // Update is called once per frame
	void Update ()
    {
        scale = new Vector3(1, 1, 1);
        acceleration = playerGivenAcceleration;
        //le coefficient de friction correspondant pour l'etat du player
        float coeff = groundDragCoeff;
        bool isGravityEnabled = true;


        // if the player is in contact with something calculate his new position
        // set isGravityEnabled and the dragCoeff
        // rescale the player if Delta speed != 0
        if (isInContactWithLeftWall || isInContactWithPlateform || isInContactWithRightWall || isInContactWithRoof)
        {
            if (isInContactWithRoof)
            {
                coeff = airDragCoeff;
                if (acceleration.y > 0)
                    acceleration.y = 0;
                if (speed.y > 0)
                {
                    speed.y = 0;
                }
            }
            else if (isInContactWithPlateform)
            {
                isGravityEnabled = false;
                if (acceleration.y < 0)
                    acceleration.y = 0;
                if (speed.y < 0)
                {
                    speed.y = 0;
                }
            }
            if (isInContactWithLeftWall)
            {
                if (acceleration.x < 0)
                    acceleration.x = 0;
                if (speed.x < 0)
                {
                    speed.x = 0;
                }
            }
            else if (isInContactWithRightWall)
            {
                if (acceleration.x > 0)
                    acceleration.x = 0;
                if (speed.x > 0)
                {
                    speed.x = 0;
                }
            }
        }
        else//no contact
        {
            coeff = airDragCoeff;
            isGravityEnabled = true;
        }
        if(isGravityEnabled)
            gravityCalculator();

        //update position
        speed = speed + (acceleration * Time.deltaTime);
        scale = new Vector3(1, scale.y+(speed.y/(2*maxSpeed)), 1);
        drag(coeff);
        position = position + (speed * Time.deltaTime);
        checkOutOfWall();
	}

    public void playerHasCollided(BoxCollider collider)
    {
        Debug.Log(collider.gameObject + " entered");


        if (collider.CompareTag(StringEnum.GetStringValue(Tags.LEFT_WALL)))
        {
            float distanceX = ((collider.bounds.size.x + move.GetComponent<Collider>().bounds.size.x) / 2);
            float wallPosition = collider.transform.position.x + collider.center.x;
            speed.x = 0;
            position = new Vector2(wallPosition + distanceX, position.y);
            isInContactWithLeftWall = true;
        }
        else if (collider.CompareTag(StringEnum.GetStringValue(Tags.RIGHT_WALL)))
        {
            float distanceX = ((collider.bounds.size.x + move.GetComponent<Collider>().bounds.size.x) / 2);
            float wallPosition = collider.transform.position.x + collider.center.x;
            speed.x = 0;
            position = new Vector2(wallPosition - distanceX, position.y);
            isInContactWithRightWall = true;
        }
        else if (collider.CompareTag(StringEnum.GetStringValue(Tags.PLATFORM)))
        {
            float distanceY = ((collider.bounds.size.y + move.GetComponent<Collider>().bounds.size.y) / 2);
            //la position du collider par rapport a l'object
            float center = collider.center.y;
            //la position de l'object portant le collider
            float positionCollider = collider.transform.position.y;
            //position du collider
            float platformPosition = center + positionCollider;
            speed.y = 0;
            position = new Vector2(position.x, platformPosition + distanceY);
            isInContactWithPlateform = true;
        }
        else if(collider.CompareTag(StringEnum.GetStringValue(Tags.ROOF)))
        {
            float distanceY = ((collider.bounds.size.y + move.GetComponent<Collider>().bounds.size.y) / 2);
            //la position du collider par rapport a l'object
            float center = collider.center.y;
            //la position de l'object portant le collider
            float positionCollider = collider.transform.position.y;
            //position du collider
            float platformPosition = center + positionCollider;
            speed.y = 0;
            position = new Vector2(position.x, platformPosition - distanceY);
            isInContactWithRoof = true;
        }

    }

    public void playerHasExitCollider(BoxCollider collider)
    {
        Debug.Log(collider.gameObject + " exited");

        if (collider.CompareTag(StringEnum.GetStringValue(Tags.LEFT_WALL)))
        {
            isInContactWithLeftWall = false;
        }
        else if(collider.CompareTag(StringEnum.GetStringValue(Tags.RIGHT_WALL)))
        { 
            isInContactWithRightWall = false;
        }
        else if (collider.CompareTag(StringEnum.GetStringValue(Tags.PLATFORM)))
        {
            isInContactWithPlateform = false;
        }
        else if(collider.CompareTag(StringEnum.GetStringValue(Tags.ROOF)))
        {
            isInContactWithRoof = false;
        }
    }

    public static void newWall(BoxCollider collider)
    {
        colliderList.Add(collider);
    }
}
