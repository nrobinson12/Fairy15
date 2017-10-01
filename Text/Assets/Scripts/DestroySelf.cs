using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    public float lifetime = 0;

	// Use this for initialization
	void Start () {

        Invoke("SelfDestruct", lifetime);
		
	}
	
	void SelfDestruct ()
    {
        Destroy(gameObject);
    }
}
