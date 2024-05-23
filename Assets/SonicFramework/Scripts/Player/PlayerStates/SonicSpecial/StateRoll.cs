using FMOD.Studio;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateRoll : StateGround
    {
        private float rollMultiplier;
        private float rollSterring;
        
        public StateRoll(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            rollMultiplier = config.rollMultiplier;
            rollSterring = config.rollSteering;
        }

        public override void Enter()
        {
            base.Enter();

            col.height = 0.2f;

            if (rb.velocity.magnitude < 10)
            {
                rb.AddForce(gameObject.transform.forward * 0.3f, ForceMode.Impulse);
            }
            
            animator.SetBool(PlayerHash.Rolling, true);
            sounds.PlaySound("Roll");
            sounds.spinDashInstance.start();
            effects.EnableJumpBall(true);
            effects.jumpBall.Animate();
        }

        public override void Exit()
        {
            base.Exit();

            col.height = 1;

            sounds.spinDashInstance.stop(STOP_MODE.ALLOWFADEOUT);
            
            animator.SetBool(PlayerHash.Rolling, false);
            effects.EnableJumpBall(false);
            player.Model.modelRenderer.enabled = true;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            player.Model.modelRenderer.enabled = false;

            if (player.Input.WasReleased(InputButton.Roll))
            {
                animator.SetBool("Landing", true);
                
                fsm.SetState<StateGround>();
            }
        }

        protected override void Move()
        {
            Vector3 velocity = rb.velocity;
            Math.SplitPlanarVector(velocity, groundNormal, out Vector3 planar, out Vector3 airVelocity);

            movementVector = planar;
            planarVelocity = planar;

            if (inputDir.magnitude > 0.2f)
            {
                turnRate = Mathf.Lerp(turnRate, rollSterring, Time.fixedDeltaTime * turnSmoothing);
                Vector3 newVelocity = Quaternion.FromToRotation(planarVelocity.normalized, inputDir.normalized) * planarVelocity;
                float Handling = turnRate * _groundHandlingAmount;
                Handling *= turnSpeedOverSpeed.Evaluate(planarVelocity.magnitude / topSpeed);
                movementVector = Vector3.Slerp(planarVelocity, newVelocity, Time.fixedDeltaTime * Handling);
            }
            
            airVelocity = Vector3.ClampMagnitude(airVelocity, 125f);
            Vector3 movementVelocity = movementVector + airVelocity;
            movementVector = Vector3.ClampMagnitude(movementVector, maxSpeed + maxSpeed * 0.25f);
            rb.velocity = movementVelocity;
        }

        protected override void SlopePhysics()
        {
            groundAngle = Vector3.Angle(groundNormal, Vector3.up);
            if (rb.velocity.magnitude < stickSpeed && groundAngle >= 89)
            {
                rb.velocity = Vector3.zero;
                detachedTimer = 0.15f;
                rb.AddForce(groundNormal * 0.15f, ForceMode.Impulse);
                groundNormal = Vector3.up;
                grounded = false;
            }
            
            bool uphill = Vector3.Dot(rb.velocity.normalized, vectorGravity) < 0;
            //AnimationCurve curve = InLoop && rb.velocity.magnitude > inLoopSpeedThreshold ? loopForceOverSpeed : slopeForceOverSpeed;
            float groundSpeedMod = slopeForceOverSpeed.Evaluate(rb.velocity.sqrMagnitude / topSpeed / topSpeed);
            Vector3 slopeForce = Vector3.ProjectOnPlane(vectorGravity, groundNormal) * (rollMultiplier * (uphill ? groundSpeedMod : 1f));
            rb.velocity += slopeForce * Time.fixedDeltaTime;
        }
    }
}