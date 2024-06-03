using FMOD.Studio;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateSpinDashCharge : StateMove
    {
        public float spinDashMaxChargeTime { get; private set; }
        public float spinDashChargeMultiplier { get; private set; }
        public float animateTimer { get; private set; }
        public float startTime { get; private set; }
        public SpinDashType type { get; private set; }
        public float spinDashChargeRate { get; private set; }
        public int counter { get; private set; }
        public float chargeTimer { get; private set; }
        private float stopSpeed;
        private float maxStopSpeed;
        private float angle;
        private float slopeTimer;

        public StateSpinDashCharge(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            spinDashMaxChargeTime = config.spinDashMaxChargeTime;
            spinDashChargeMultiplier = config.spinDashChargeMultiplier;
            type = config.spinDashType;
            spinDashChargeRate = config.spinDashChargeRate;

            stopSpeed = 30f;
        }

        public override void Enter()
        {
            chargeTimer = 0;
            animateTimer = 0;
            counter = 0;
            slopeTimer = 0;
            sounds.spinDashInstance.start();
            col.height = 0.2f;
            effects.EnableJumpBall(true);
            effects.Animate();
            
            CheckGround(-_groundCheck.up, out var ground);
            groundNormal = ground.normal;
            
            maxStopSpeed = Mathf.Max(stopSpeed, rb.velocity.magnitude * 0.5f);
            
            //animator.Play("Roll");
        }

        public override void Exit()
        {
            col.height = 1;
            sounds.spinDashInstance.stop(STOP_MODE.ALLOWFADEOUT);
            effects.EnableJumpBall(false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            player.Model.modelRenderer.enabled = false;
            
            if (type is SpinDashType.Manual)
            {
                Manual();
            }
            else Auto();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (angle >= 90)
            {
                rb.velocity += Vector3.up * (config.gravity * 0.1f * Time.deltaTime);
            }
            
            //Vector3 flat = new Vector3(0, angle >= 90 ? -4 : 0, 0);
            Vector3 flat = new Vector3(0, -4, 0);
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.ProjectOnPlane(flat, groundNormal), maxStopSpeed * Time.deltaTime);
            if (CheckGround(-_groundCheck.up, out var ground))
            {
                groundNormal = ground.normal;
                angle = Vector3.Angle(groundNormal, Vector3.up);
                
                rb.position = ground.point + ground.normal * col.height / 2;
                transformNormal = Vector3.Slerp(transformNormal, groundNormal, Time.fixedDeltaTime * 20f);
            }
            else
            {
                fsm.SetState<StateAir>();
            }

            if (angle >= 89)
            {
                slopeTimer += Time.deltaTime;

                if (slopeTimer > 3)
                {
                    transform.up = Vector3.up;
                    rb.AddForce(groundNormal * 0.1f, ForceMode.Impulse);
                    fsm.SetState<StateAir>();
                }
            }
            
            Rotate();
        }

        protected override void Rotate()
        {
            transformNormal = Vector3.Slerp(transformNormal, groundNormal, Time.fixedDeltaTime * 6f);

            if (inputDir.magnitude > 0)
            {
                Vector3 lookDir = gameObject.transform.forward;
                lookDir = Vector3.Slerp(gameObject.transform.forward, Vector3.ProjectOnPlane(inputDir, groundNormal).normalized, Time.fixedDeltaTime * 12f);
                Quaternion rotation = Quaternion.LookRotation(lookDir, transformNormal);
                gameObject.transform.rotation = rotation;
            }
        }

        private void Manual()
        {
            if (player.Input.WasPressed(InputButton.Charge) && chargeTimer < spinDashMaxChargeTime && Time.time >= startTime)
            {
                startTime = Time.time + 1f / spinDashChargeRate;
                
                float value = 1 * spinDashChargeMultiplier;
                chargeTimer += value;
                sounds.PlaySound("Roll");
                effects.Animate();
            }
        }

        private void Auto()
        {
            if (chargeTimer < spinDashMaxChargeTime)
            {
                float value = Time.deltaTime * spinDashChargeMultiplier;
                chargeTimer += value;
                animateTimer += value;
            }
            
            if (animateTimer >= 0.5f && counter < Mathf.Round(spinDashMaxChargeTime))
            {
                effects.Animate();
                sounds.PlaySound("Roll");
                counter++;
                animateTimer = 0;
            }
        }

        protected override void CheckButtons()
        {
            if (player.Input.WasReleased(InputButton.Roll))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(gameObject.transform.forward * chargeTimer, ForceMode.Impulse);
                fsm.SetState<StateRoll>();
            }
        }

        protected override void Move()
        {
        }
    }
}