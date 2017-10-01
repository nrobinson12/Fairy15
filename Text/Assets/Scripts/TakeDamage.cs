using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour {

    public GameObject explosion;

    void OnTriggerEnter(Collider impactObject)
    {
        if (impactObject.tag != "ChaseSensor" && impactObject.tag != "EnemyAI")
        {
            //Debug.Log("DAMAGE TAKEN");
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(transform.parent.parent.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
