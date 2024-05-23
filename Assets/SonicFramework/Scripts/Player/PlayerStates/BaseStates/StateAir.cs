
using SonicFramework.CharacterFlags;
using SonicFramework.StateMachine;
using UnityEngine;
using Physics = UnityEngine.Physics;

namespace SonicFramework.PlayerStates
{
    public class StateAir : StateMove
    {
        protected bool useGravity = true;
        public Transform target;
        
        private float _airHandlingAmount;

        protected float airTime;
        protected float homingFindRadius;
        protected float homingFindDistance;
        protected LayerMask homingMask;
        protected LayerMask blockMask;
        
        public StateAir(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            _airHandlingAmount = config.airHandlingAmount;
            
            homingFindRadius = config.homingFindRadius;
            homingFindDistance = config.homingFindDistance;
            homingMask = config.homingMask;
            blockMask = config.groundMask;
        }

        public override void Enter()
        {
            base.Enter();
            
            airTime = 0f;
            animator.SetBool(PlayerHash.Falling, true);
        }

        public override void Exit()
        {
            base.Exit();
            
            animator.Play("Falling Blend");
            animator.SetBool(PlayerHash.Falling, false);
            animator.SetFloat(PlayerHash.VerticalVelocity, 0);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            vectorGravity = new Vector3(0, gravity, 0);
            
            if (player.Flags.Check(Flag.SlowFall)) vectorGravity *= 0.75f;

            airTime += Time.deltaTime;
            animator.SetFloat(PlayerHash.VerticalVelocity, rb.velocity.y);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Air();
            Rotate();
        }

        protected virtual void Air()
        {
            if (!CheckGround(-_groundCheck.up, out _))
            {
                groundNormal = -vectorGravity.normalized;
                Move();
                target = FindClosestHomingTarget();
                if (useGravity) rb.velocity += vectorGravity * Time.fixedDeltaTime;

                transformNormal = Vector3.Slerp(transformNormal, Vector3.up, Time.fixedDeltaTime * 4f);
            }
            else
            {
                if (fsm.CurrentState is not StateJumpDash)
                {
                    if (!PredictSpring())
                    {
                        animator.SetBool("Landing", true);
                
                        fsm.SetState<StateGround>();
                    }
                }
            }
        }

        protected override void Move()
        {
            Vector3 velocity = rb.velocity;
            Math.SplitPlanarVector(velocity, groundNormal, out Vector3 planar, out Vector3 airVelocity);

            movementVector = planar;
            planarVelocity = planar;

            if (inputDir.magnitude > 0.2f && !braking)
            {
                turnRate = Mathf.Lerp(turnRate, turnSpeed, Time.fixedDeltaTime * turnSmoothing);
                if (planarVelocity.magnitude < topSpeed)
                    planarVelocity += inputDir * (accelRate * Time.fixedDeltaTime);
                float handling = turnRate * _airHandlingAmount;
                //handling *= turnSpeedOverSpeed.Evaluate(planarVelocity.magnitude / topSpeed);
                movementVector = Vector3.Lerp(planarVelocity, inputDir.normalized * planarVelocity.magnitude, 
                    Time.fixedDeltaTime * handling);
            }
            else
            {
                // float f = Mathf.Lerp(maxDecelRate, minDecelRate, movementVector.magnitude / topSpeed);
                // if (movementVector.magnitude > 1f)
                //     movementVector = Vector3.MoveTowards(movementVector, Vector3.zero, Time.fixedDeltaTime * f);
                // else
                // {
                //     movementVector = Vector3.zero;
                // }
            }
            
            airVelocity = Vector3.ClampMagnitude(airVelocity, 400f);
            Vector3 movementVelocity = movementVector + airVelocity;
            rb.velocity = movementVelocity;
        }

        public override void BaseUpdate()
        {
            base.BaseUpdate();

            if (player.Input.WasPressed(InputButton.LightSpeedDash) && closestRing != null)
            {
                transform.position = closestRing.position;
                
                fsm.SetState<StateLightSpeedDash>();
            }
        }

        protected override void CheckButtons()
        {
            base.CheckButtons();
            
            if (player.Input.WasPressed(InputButton.Roll))
            {
                fsm.SetState<StateDropDash>();
            }
            
            if (player.Input.WasPressed(InputButton.Jump))
            {
                if (target == null)
                {
                    fsm.SetState<StateJumpDash>();
                }
                else
                {
                    if (fsm.GetState<StateHoming>(out var state))
                    {
                        target.TryGetComponent(out HomingTarget h);
                        state.SetTarget(h);
                    }
                    
                    fsm.SetState<StateHoming>();
                }
            }
            
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

        protected virtual Transform FindClosestHomingTarget()
        {
            Vector3 origin = transform.position;
            Vector3 dir = inputDir == Vector3.zero ? transform.forward : inputDir;
            RaycastHit[] targetsInRange = Physics.SphereCastAll(origin, homingFindRadius, dir, homingFindDistance, homingMask);
            Transform closestTarget = null;
            float distance = 1f;
            foreach (RaycastHit t in targetsInRange)
            {
                Transform target = t.transform;
                Vector3 direction = target.position - origin;
                bool facing = Vector3.Dot(direction.normalized, transform.forward) > 0.4f;
                float targetDistance = (direction.sqrMagnitude / homingFindRadius) / homingFindRadius;
                if (targetDistance < distance && facing)
                {
                    if (!Physics.Linecast(origin, target.position + target.up * 0.2f, blockMask))
                    {
                        closestTarget = target;
                        distance = targetDistance;
                    }
                    
                    // closestTarget = target;
                    // distance = targetDistance;
                }
            }
    
            return closestTarget;
        }
    }
}