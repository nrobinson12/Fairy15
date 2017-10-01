using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CameraControl : NetworkBehaviour {

	public float forwardDist;
	public float upDist;
	public float bias;
	public float minTrailBack;

    public GameObject cameraPivot;

    void Start ()
    {
        //Camera.main.transform.position = cameraPivot.transform.position + new Vector3(0, 7.95f, -20.48f);

        //Vector3 moveCamTo = transform.position - transform.forward * forwardDist + Vector3.up * upDist;
        //Camera.main.transform.position = Camera.main.transform.position + moveCamTo;
        //Camera.main.transform.LookAt(transform.position + transform.forward * minTrailBack);
    }

    // Update is called once per frame
    void FixedUpdate () {

		if (!isLocalPlayer) {
			return;
		}
		        
    	//Camera control script
        //Vector3 moveCamTo = transform.position - transform.forward * forwardDist + Vector3.up * upDist;
        //Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        //Camera.main.transform.LookAt(transform.position + transform.forward * minTrailBack);

        Vector3 moveCamTo = cameraPivot.transform.position - cameraPivot.transform.forward * forwardDist + Vector3.up * upDist;
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        Camera.main.transform.LookAt(cameraPivot.transform.position + cameraPivot.transform.forward * minTrailBack);

    }
}
