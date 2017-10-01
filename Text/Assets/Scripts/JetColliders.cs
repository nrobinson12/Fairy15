using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetColliders : MonoBehaviour {


    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collidion");

        //transform.parent.parent.GetComponent<CollisionControl>().HandleCollision(collision);

        // add burn effect to this location - later have damage counters for each part of the plane
    }

	// Use this for initialization
	void Start () {
        Debug.Log("running Jet colliders script");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
