using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour {


    public bool isInContactWithWall;
    private ParticleSystem.EmissionModule emission;
	// Use this for initialization
	void Start () {
        emission = this.gameObject.GetComponent<ParticleSystem>().emission;
    }
	
	// Update is called once per frame
	void Update () {
	    if(isInContactWithWall)
        {
            emission.enabled = true;
        }
        else
        {
            emission.enabled = false;

        }
    }
}
