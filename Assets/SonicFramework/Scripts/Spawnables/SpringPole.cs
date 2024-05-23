using System;
using FMODUnity;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    public class SpringPole : PlayerContactable
    {
        [SerializeField] private bool alwaysMaxForce;

        [SerializeField] private EventReference soundReference;
        
        private float timer;

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }

        public override void OnContact()
        {
            if (timer == 0)
            {
                // Vector3 dir = Vector3.Normalize(transform.position - player.transform.position);
                // float dot = Vector3.Dot(dir, Vector3.up);
                //
                // if (dot > 0)
                // {
                //     Debug.Log("up");
                //     
                //     player.fsm.rb.ResetVelocity();
                //     player.fsm.rb.AddForce(transform.up * 1f, ForceMode.Impulse);
                //     player.fsm.SetState<StateAir>();
                // }
                // else if (dot < 0)
                // {
                //     Debug.Log("down");
                // }

                if (player.transform.position.y > transform.position.y)
                {
                    if (alwaysMaxForce)
                    {
                        player.fsm.rb.ResetVelocity();
                        player.fsm.rb.AddForce(transform.up * 0.75f, ForceMode.Impulse);
                        player.fsm.SetState<StateAir>();
                    }
                    else
                    {
                        float distance =
                            Vector3.Distance(transform.position, player.transform.position);

                        float min = 0.5f;
                        float max = 0.75f;
                        float force = Mathf.Lerp(min, max, distance / 3f);
                        
                        player.fsm.rb.ResetVelocity();
                        player.fsm.rb.AddForce(transform.up * force, ForceMode.Impulse);
                        player.fsm.SetState<StateAir>();
                    }
                    
                    RuntimeManager.PlayOneShot(soundReference, transform.position);
                }
            }

            timer = 0.02f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.up * 0.25f / 0.02f);
        }
    }
}