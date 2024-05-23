using System;
using UnityEngine;

namespace SonicFramework
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsRing : Ring
    {
        private Rigidbody rb;

        private float deactiveTimer;
        private float groundForce;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            var m = new PhysicMaterial
            {
                bounciness = 1f,
                dynamicFriction = 0.2f
            };

            deactiveTimer = 0.5f;
            groundForce = 4f;

            GetComponent<Collider>().material = m;
        }

        private void Update()
        {
            if (deactiveTimer > 0)
            {
                deactiveTimer -= Time.deltaTime;
            }
            else
            {
                deactiveTimer = 0;
            }
        }

        public override void OnContact()
        {
            if (deactiveTimer == 0)
            {
                base.OnContact();
            }
        }

        public void ApplyForce(Vector3 force)
        {
            rb.AddForce(force);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (groundForce == 0)
            {
                return;
            }
            
            if (!other.transform.CompareTag("Player"))
            {
                rb.AddForce(Vector3.up * groundForce, ForceMode.Impulse);

                groundForce *= 0.75f;

                if (groundForce <= 0.01f)
                {
                    groundForce = 0;
                }
            }
        }
    }
}