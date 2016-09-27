using UnityEngine;
using System.Collections.Generic;

public class LevelFactory : MonoBehaviour
{
    /// <summary>
    /// The prefab to build the plateforms
    /// </summary>
    public GameObject platformPrefab;

    /// <summary>
    /// List of the position of the platforms
    /// Position : betweeen 0 and 1
    /// </summary>
    /// 
    public List<Vector3> posPlatforms;


    /// <summary>
    /// List of the scale of the platforms
    /// Scale : whatever you want
    /// </summary>
    public List<Vector3> scalePlatforms;

    // Use this for initialization
    void Start ()
    {
        // XML read of the platforms ?

        if (posPlatforms.Count != scalePlatforms.Count)
            Debug.Log("CRITICAL pos != scale");

        initWalls();
        initPlatforms();
	}
	
    /// <summary>
    /// Initialize the walls, ground, and roof
    /// </summary>
    void initWalls()
    {
        GameObject ground = Instantiate(platformPrefab);
        ground.transform.parent = this.transform;
        Vector3 posGround = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        ground.transform.position = new Vector3(0, posGround.y + ground.GetComponent<Collider>().bounds.size.y / 2, 0);
        ground.transform.localScale = new Vector3(100, 1, 1);
        ground.gameObject.name = "Ground";


        GameObject roof = Instantiate(platformPrefab);
        roof.transform.parent = this.transform;
        Vector3 posRoof = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        roof.transform.position = new Vector3(0, posRoof.y - ground.GetComponent<Collider>().bounds.size.y / 2, 0);
        roof.transform.localScale = new Vector3(100, 1, 1);
        roof.gameObject.name = "Roof";


        GameObject leftWall = Instantiate(platformPrefab);
        leftWall.transform.parent = this.transform;
        Vector3 posLeftWall = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        leftWall.transform.position = new Vector3(posLeftWall.x + leftWall.GetComponent<Collider>().bounds.size.y / 2, 0 , 0);
        leftWall.transform.localScale = new Vector3(100, 1, 1);
        leftWall.transform.Rotate(new Vector3(0, 0, 90));
        leftWall.gameObject.name = "LeftWall";


        GameObject rightWall = Instantiate(platformPrefab);
        rightWall.transform.parent = this.transform;
        Vector3 posRightWall = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0));
        rightWall.transform.position = new Vector3(posRightWall.x - rightWall.GetComponent<Collider>().bounds.size.y / 2, 0, 0);
        rightWall.transform.localScale = new Vector3(100, 1, 1);
        rightWall.transform.Rotate(new Vector3(0, 0, -90));
        rightWall.gameObject.name = "RightWall";

    }

    /// <summary>
    /// Initialize the platforms : scale and positions
    /// </summary>
    void initPlatforms()
    {
        for(int i = 0; i< posPlatforms.Count; i++)
        {
            Vector3 pos = posPlatforms[i];
            Vector3 scale = scalePlatforms[i];

            GameObject platform = Instantiate(platformPrefab);
            platform.transform.parent = this.transform;
            platform.gameObject.name = "Platform";
            Vector3 posPlatform = Camera.main.ViewportToWorldPoint(pos);
            platform.transform.position = new Vector3(posPlatform.x, posPlatform.y, 0);
            platform.transform.localScale = scale;
        }
    }

}
