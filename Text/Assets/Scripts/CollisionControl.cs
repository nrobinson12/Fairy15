using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CollisionControl : NetworkBehaviour {

    

    public float maxHealth;
    public int bulletDamage;
    public float killingSpeed;
    public float surfaceDamage;

    public GameObject deathExplosion;
    public GameObject collideExplosion;

    [SyncVar(hook = "ChangeHealthBar")]
    public float health;
    public RectTransform healthBar;

    private Rigidbody rb;
    private NetworkStartPosition[] spawnPoints;


    // Use this for initialization
    void Start() {
        rb = transform.GetComponent<Rigidbody>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        

    }


    // this is for colliding with pre-built aircraft colliders (not triggers)
    void OnCollisionEnter(Collision collision)
    {
        if (health > 0)
        {

            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag != "Bullet") // must be collision against the environment
            {

                // do a little explosion
                Instantiate(collideExplosion, collision.contacts[0].point, transform.rotation);

                Debug.Log("Collision");

                TakeDamage(surfaceDamage);

                Debug.Log("HEALTH: " + health);
            }

        }
    }


    void OnTriggerEnter(Collider impactObject)
    {
        //Debug.Log("HEALTH");
        if (health > 0)
        {
            Debug.Log(impactObject.tag);
            if (impactObject.tag == "Bullet")
            {
                TakeDamage(bulletDamage);
                Destroy(impactObject);
            }
            
            Debug.Log("HEALTH: " + health);

        }
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }


    void ExplodeAndCrash()
    {
        // instantiate explosion, fire and smoke
        Instantiate(deathExplosion, transform.position, transform.rotation, transform);

        // disable controls and enable gravity on rigidbody
        transform.GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneController>().enabled = false;
        transform.GetComponent<UnityStandardAssets.Vehicles.Aeroplane.AeroplaneUserControl2Axis>().enabled = false;
        rb.useGravity = true;
        //rb.mass = 1000;
        rb.AddForce(Vector3.down * 5000);

        //Physics.gravity = Vector3.down * 100;

        transform.GetChild(1).gameObject.SetActive(false);

    }

    void ChangeHealthBar(float myHealth) {
        Debug.Log(myHealth);
        healthBar.sizeDelta = new Vector2((myHealth / maxHealth) * 100, healthBar.sizeDelta.y);
    }

    void TakeDamage(float amount){
        if (!isServer) {
            return;
        }
        health -= amount;
        if (health <= 0) {
            ExplodeAndCrash();
            health = maxHealth;
            //score
            RpcRespawn();
        }
        //ChangeHealthBar();
    }

}
