using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateDamage : StateMove
    {
        private float timer;
        private float layTimer;
        private float detachedTimer;
        private bool grounded;
        private bool layed;
        private bool animated;
        
        public StateDamage(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
        }

        public override void Enter()
        {
            base.Enter();

            grounded = false;
            layed = false;
            animated = false;

            detachedTimer = 0.15f;
            
            animator.SetBool("InDamage", true);
            // animator.Play("Movement Blend");
            // animator.CrossFadeInFixedTime("Lay", 0.25f, 0);
            
            //transform.position += Vector3.up * 0.2f;
            
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * config.knockBackUpwardsForce, ForceMode.Impulse);
            rb.AddForce(-transform.forward * config.knockBackBackwardsForce, ForceMode.Impulse);
        }

        public override void Exit()
        {
            base.Exit();
            
            animator.SetBool("InDamage", false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (grounded)
            {
                if (!animated)
                {
                    //animator.Play("Standup");
                    timer = 0.75f;
                    animated = true;
                }
                
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    fsm.SetState<StateGround>();
                }
            }

            if (layed && !grounded)
            {
                if (layTimer > 0)
                {
                    layTimer -= Time.deltaTime;
                }
                else
                {
                    timer = 0.75f;
                    layTimer = 0;
                    grounded = true;
                }
            }
            
            if (detachedTimer > 0)
            {
                detachedTimer -= Time.deltaTime;
            }
            else
            {
                detachedTimer = 0;
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (CheckGround(-_groundCheck.up, out var hit))
            {
                if (detachedTimer == 0)
                {
                    if (!layed)
                    {
                        layTimer = 0.5f;
                        rb.velocity = Vector3.zero;
                        layed = true;
                    }

                    Vector3 target = hit.point + hit.normal * col.height / 2f;
                    Vector3 lerped = Vector3.Lerp(transform.position, target, 50 * Time.fixedDeltaTime);
                    rb.position = lerped;
                }
            }
            else
            {
                vectorGravity = new Vector3(0, gravity * 0.45f, 0);
                rb.velocity += vectorGravity * Time.fixedDeltaTime;
            }
        }

        protected override void Move()
        {
            
        }
    }
}