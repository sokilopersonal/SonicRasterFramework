using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateJump : StateAir
    {
        private float _jumpForce;
        private float jumpTimer;
        
        private float _delay = 0.2f;
        private float _delayTimer;

        private bool ballAnimated;
        
        public StateJump(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            _jumpForce = config.jumpForce;
        }

        public override void Enter()
        {
            base.Enter();

            _delayTimer = _delay;
            jumpTimer = 0;

            ballAnimated = false;

            col.height = 0.2f;
            col.center = new Vector3(0, -0.1f, 0);
            sounds.PlaySound("Jump");
            
            animator.SetBool(PlayerHash.Jumped, true);

            if (rb.velocity.magnitude > 7)
            {
                int index = Random.Range(1, 3);
                animator.Play($"Hop {index}");
            }
            else
            {
                animator.Play("ShortHop");
            }

            //rb.position += gameObject.transform.up * 0.5f;
            rb.AddForce(_jumpForce * gameObject.transform.up, ForceMode.Impulse);
        }

        public override void Exit()
        {
            base.Exit();

            col.height = 1f;
            col.center = new Vector3(0, 0.5f, 0);
            
            animator.SetBool(PlayerHash.Jumped, false);
            effects.EnableJumpBall(false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (_delayTimer > 0)
            {
                _delayTimer -= Time.deltaTime;
            }
            else
            {
                _delayTimer = 0;
            }

            if (player.Input.IsPressed(InputButton.Jump))
            {
                jumpTimer += Time.deltaTime;

                if (jumpTimer >= 0.1f)
                {
                    if (!ballAnimated)
                    {
                        effects.EnableJumpBall(true);
                        effects.Animate();
                        ballAnimated = true;
                    }
                }
            }

            if (rb.velocity.magnitude <= 7)
            {
                if (airTime >= 0.2f)
                {
                    if (!ballAnimated)
                    {
                        effects.EnableJumpBall(true);
                        effects.Animate();
                        ballAnimated = true;
                    }
                }
            }
            
            animator.SetFloat("JumpTimer", jumpTimer);

            if (player.Input.WasReleased(InputButton.Jump) && rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 2, rb.velocity.z);
            }

            if (airTime >= 0.9f)
            {
                animator.SetFloat("JumpTimer", 0f);
                fsm.SetState<StateAir>();
            }
        }

        protected override void Air()
        {
            if (!CheckGround(-_groundCheck.up,out _))
            {
                groundNormal = -vectorGravity.normalized;
                Move();

                target = FindClosestHomingTarget();
                
                rb.velocity += vectorGravity * Time.fixedDeltaTime;

                transformNormal = Vector3.Slerp(transformNormal, Vector3.up, Time.fixedDeltaTime * 15f);
            }
            
            if (CheckGround(-_groundCheck.up, out _) && _delayTimer <= 0)
            {
                animator.SetBool("Landing", true);
                
                fsm.SetState<StateGround>();
            }
        }
    }
}