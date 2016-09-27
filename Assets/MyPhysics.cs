using UnityEngine;
using System.Collections;

public class MyPhysics : MonoBehaviour
{

    public Vector2 playerGivenSpeed;
    private Vector2 speed;
    public Vector2 position { get; private set; }
    private MovePlayer move;

    /// <summary>
    /// The gravity of the world
    /// </summary>
    [SerializeField]
    float gravity;

	// Use this for initialization
	void Start ()
    {
        speed = new Vector2(0,0);

        move = this.gameObject.GetComponent<MovePlayer>();
	}
	
    /// <summary>
    /// calcul de la nouvelle acceleration en prenant compte de la gravité
    /// </summary>
	private void gravityCalculator()
    {
        speed.y -= gravity;
    }
    
    // Update is called once per frame
	void Update ()
    {
        speed = playerGivenSpeed;

        bool isGravityEnabled = false;
        if(move.isInContactWithPlatform)
        {
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

        Debug.Log("input :"+playerGivenSpeed);
        //update position
        Debug.Log("speed :"+speed);
        position = position + (speed * Time.deltaTime);
        Debug.Log("position :" + position);
	}
}
