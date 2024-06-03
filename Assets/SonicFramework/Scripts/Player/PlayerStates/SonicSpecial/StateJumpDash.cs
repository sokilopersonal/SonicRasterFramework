using SonicFramework.CharacterFlags;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateJumpDash : StateAir
    {
        private float jumpDashForce;
        private float jumpDashTimer;
        private float velocityFadeTimer;
        
        public StateJumpDash(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            jumpDashForce = config.jumpDashForce;
        }

        public override void Enter()
        {
            base.Enter();

            rb.velocity = Vector3.zero;
            jumpDashTimer = 0f;
            velocityFadeTimer = 0f;

            effects.trail.emitTime = 99f;
            effects.trail.emit = true;
            
            player.Flags.Add(Flag.SlowFall);
            
            animator.SetBool("JumpDash", true);
            effects.EnableJumpBall(true);
            
            sounds.PlaySound("Homing");
        }

        public override void Exit()
        {
            base.Exit();

            effects.trail.emitTime = 2;
            
            animator.SetBool("JumpDash", false);
            effects.EnableJumpBall(true);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            player.Input.movementInput = Vector3.zero;

            if (jumpDashTimer <= config.jumpDashMaxTimer)
            {
                rb.ResetVerticalVelocity();
                rb.velocity = transform.forward * Mathf.Max(config.jumpDashForce, rb.velocity.magnitude);
            
                jumpDashTimer += Time.deltaTime;
                
                velocityFadeTimer += Time.deltaTime;  
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (CheckGround(-_groundCheck.up, out var hit))
            {
                fsm.SetState<StateGround>();
            }

            if (CheckGround(transform.forward, out _))
            {
                fsm.SetState<StateAir>();
            }
        }

        protected override void Rotate()
        {
            transformNormal = Vector3.Slerp(transformNormal, Vector3.up, Time.fixedDeltaTime * 12f);

            if (!Math.IsApproximate(rb.velocity, Vector3.zero, 0.5f))
            {
                Vector3 lookDir = transform.forward;
                lookDir = Vector3.Slerp(transform.forward, Vector3.ProjectOnPlane(rb.velocity.normalized, transformNormal), Time.deltaTime * 7f);
                Quaternion rotation = Quaternion.LookRotation(lookDir, transformNormal);
                gameObject.transform.rotation = rotation;
            }
        }

        protected override void CheckButtons()
        {
            if (player.Input.WasPressed(InputButton.LightSpeedDash))
            {
                closestRing = GetNearestRing();
                
                if (closestRing == null)
                {
                    return;
                }
                
                transform.position = closestRing.position;
                
                fsm.SetState<StateLightSpeedDash>();
            }
        }
        
        private float EasingInverseSquared(float x)
        {
            return 1-(1-x)*(1-x);
        }
    }
}