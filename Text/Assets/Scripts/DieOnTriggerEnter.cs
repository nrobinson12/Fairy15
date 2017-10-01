using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTriggerEnter : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("entered co");
        Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
