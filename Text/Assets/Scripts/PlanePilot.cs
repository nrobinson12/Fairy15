using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePilot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("STARTED");
	}
	
	// Update is called once per frame
	void Update () {

        transform.position += transform.forward * Time.deltaTime * 50.0f;

        // add max roll/yaw/pitch whatever
        transform.Rotate(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));

        // Check collision with ground
        float curTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        if ( curTerrainHeight > transform.position.y )
        {
            transform.position = new Vector3(transform.position.x, curTerrainHeight, transform.position.z);
        }
	}
}
