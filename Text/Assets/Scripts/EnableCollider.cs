using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCollider : MonoBehaviour {

    public float delay = 0;

	// Use this for initialization
	void Start () {

        Invoke("Trigger", delay);
	}
	
	void Trigger ()
    {
        transform.GetComponent<BoxCollider>().enabled = true;
    }
}
