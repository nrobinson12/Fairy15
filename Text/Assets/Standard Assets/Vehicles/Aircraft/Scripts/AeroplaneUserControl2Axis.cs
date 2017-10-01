using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


namespace UnityStandardAssets.Vehicles.Aeroplane
{
    [RequireComponent(typeof (AeroplaneController))]
    public class AeroplaneUserControl2Axis : NetworkBehaviour // netbehav changes it!
    {
        public int boostMultFactor = 0;

        // these max angles are only used on mobile, due to the way pitch and roll input are handled
        public float maxRollAngle = 80;
        public float maxPitchAngle = 80;

        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public float fireSpeed;
        public float fireRate;
        float nextFireTime;
        public float bulletLifetime = 4f;

        public float boostDuration;
        public float boostRegenFactor;
        private float currentBoostingTime;

        // reference to the aeroplane that we're controlling
        private AeroplaneController m_Aeroplane;
        private AudioSource asrc;


        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Aeroplane = GetComponent<AeroplaneController>();
            asrc = GetComponent<AudioSource>();

            currentBoostingTime = boostDuration;
        }


        private void FixedUpdate()
        {
          
            // Check that player is current local player    
            if (!isLocalPlayer) {
                return;
            }

            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            float roll = CrossPlatformInputManager.GetAxis("Horizontal");
            float pitch = CrossPlatformInputManager.GetAxis("Vertical");
            bool airBrakes = CrossPlatformInputManager.GetButton("Fire1");
            bool boost = CrossPlatformInputManager.GetButton("Fire2");
            bool shoot = CrossPlatformInputManager.GetButton("Jump");

            if (shoot) 
            {
                CmdFire();
            }


            float throttle = airBrakes ? -1 : 1; //boost ? boostMultFactor : 1;

            // if boost is pressed and current time is > [threshold], decrease current boosting time by
            // deltatime.
            // if boost is not pressed, increase current time by deltatime * [factor]
            if (boost && currentBoostingTime > 0)
            {
                Debug.Log("SUPER SPEEEED");
                currentBoostingTime -= Time.deltaTime;

                throttle = boostMultFactor;
            }
            else if (currentBoostingTime < boostDuration)
            {
                currentBoostingTime += Time.deltaTime * boostRegenFactor;
            }
            //Debug.Log("Current boost time: " + currentBoostingTime);



            // Pass the input to the aeroplane
            m_Aeroplane.Move(roll, pitch, 0, throttle, airBrakes);
        }

        // Called on the server, spawned on the clients
        [Command]
        void CmdFire() 
        {
            if (Time.time > nextFireTime)
            {
                asrc.Play();

                var bullet = (GameObject)Instantiate(
                    bulletPrefab,
                    bulletSpawn.position + new Vector3(-3f, 0, 0),
                    bulletSpawn.rotation);

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * fireSpeed;
                NetworkServer.Spawn(bullet);
                nextFireTime = Time.time + fireRate;
                Destroy(bullet, bulletLifetime);

                var bullet1 = (GameObject)Instantiate(
                    bulletPrefab,
                    bulletSpawn.position + new Vector3(3f,0,0),
                    bulletSpawn.rotation);

                bullet1.GetComponent<Rigidbody>().velocity = bullet1.transform.forward * fireSpeed;
                NetworkServer.Spawn(bullet1);
                nextFireTime = Time.time + fireRate;
                Destroy(bullet1, bulletLifetime);
            }
        }


        private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
        {
            // because mobile tilt is used for roll and pitch, we help out by
            // assuming that a centered level device means the user
            // wants to fly straight and level!

            // this means on mobile, the input represents the *desired* roll angle of the aeroplane,
            // and the roll input is calculated to achieve that.
            // whereas on non-mobile, the input directly controls the roll of the aeroplane.

            float intendedRollAngle = roll*maxRollAngle*Mathf.Deg2Rad;
            float intendedPitchAngle = pitch*maxPitchAngle*Mathf.Deg2Rad;
            roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
            pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);

            // similarly, the throttle axis input is considered to be the desired absolute value, not a relative change to current throttle.
            float intendedThrottle = throttle*0.5f + 0.5f;
            throttle = Mathf.Clamp(intendedThrottle - m_Aeroplane.Throttle, -1, 1);
        }
    }
}
