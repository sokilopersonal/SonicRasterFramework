using SonicFramework.CharacterFlags;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateGround : StateMove
    {
        public float groundAngle;
        private Surface currentSurface;

        private float lastAngle;
        protected float _groundHandlingAmount;
        private float fullStopThreshold;
        private float landingTimer;
        private float idleTimer;
        private float idleActivateThreshold;
        private float chargeTimer;
        private float movementTurn;
        
        protected float detachedTimer;

        public StateGround(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            _groundHandlingAmount = config.groundHandlingAmount;
            fullStopThreshold = config.fullStopThreshold;

            idleActivateThreshold = 3;
        }

        public override void Enter()
        {
            base.Enter();
            
            effects.EnableJumpBall(false);
            player.Flags.Remove(Flag.SlowFall);
            Debug.Log("removed flag");
            
            idleTimer = 0;
            landingTimer = 0.275f;
            chargeTimer = 0;
            
            UpdateGroundNormal();

            if (fsm.LastState is StateAir)
            {
                sounds.PlaySound("Land");
            }
            else if (fsm.LastState is StateJumpDash)
            {
                transformNormal = Vector3.up;
            }

            animator.SetBool(PlayerHash.Grounded, true);
        }

        public override void Exit()
        {
            base.Exit();
            
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            animator.SetBool("Idling", false);
            animator.SetBool("Braking", false);
            
            animator.SetBool(PlayerHash.Grounded, false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            if (player.fsm.CurrentState is not StateRoll)
            {
                if (player.Input.IsPressed(InputButton.Roll))
                {
                    chargeTimer += Time.deltaTime;

                    if (chargeTimer >= 0.15f)
                    {
                        fsm.SetState<StateSpinDashCharge>();
                    }
                }

                if (player.Input.WasReleased(InputButton.Roll))
                {
                    if (chargeTimer < 0.15f)
                    {
                        fsm.SetState<StateRoll>();
                    }
                }
            }
            
            if (idleTimer <= idleActivateThreshold)
            {
                idleTimer += Time.deltaTime;
            }
            else
            {
                animator.SetBool("Idling", true);
                animator.SetInteger("IdleIndex", Random.Range(0, 4));
                
                idleTimer = 0;
            }

            if (landingTimer > 0)
            {
                landingTimer -= Time.deltaTime;
            }
            else 
            {
                animator.SetBool("Landing", false);
            }

            if (detachedTimer > 0)
            {
                detachedTimer -= Time.deltaTime;
            }
            else
            {
                detachedTimer = 0;
            }

            // if (rb.velocity.magnitude < fullStopThreshold && !player.Input.IsAnyButtonPressed())
            // {
            //     rb.constraints = RigidbodyConstraints.FreezeAll;
            // }
            // else
            // {
            //     rb.constraints = RigidbodyConstraints.FreezeRotation;
            // }
            
            animator.SetFloat(PlayerHash.GroundVelocity, Mathf.Clamp(rb.velocity.magnitude, 0, 70f));
            movementTurn = Mathf.Lerp(movementTurn, player.fsm.LocalVelocity.x * 0.2f, 4 * Time.deltaTime);
            animator.SetFloat("MovementTurn", movementTurn);
        }

        protected override void CheckButtons()
        {
            // if (player.Input.gameInput.Gameplay.Jump.WasPressedThisFrame())
            // {
            //     fsm.SetState<StateJump>();
            // }

            if (player.Input.WasPressed(InputButton.Jump))
            {
                fsm.SetState<StateJump>();
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

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (CheckGround(-_groundCheck.up, out var ground))
            {
                lastAngle = groundAngle;
                
                ground.transform.TryGetComponent(out Surface surface);
                currentSurface = surface;

                groundNormal = ground.normal;
                transformNormal = Vector3.Slerp(transformNormal, groundNormal, Time.fixedDeltaTime * 12f);
                
                Move();

                if (rb.velocity.magnitude > stickSpeed)
                {
                    SlopePrediction(Time.fixedDeltaTime);
                }

                if (!braking) SlopePhysics();
                groundAngle = Vector3.Angle(groundNormal, Vector3.up);
                
                Rotate();
                
                Vector3 target = ground.point + ground.normal * col.height / 2;
                if (detachedTimer == 0)
                {
                    float epsilon = 0.01f;
                    if (Mathf.Abs(lastAngle - groundAngle) >= epsilon)
                    {
                        //rb.position = target + Vector3.ProjectOnPlane(transform.forward, groundNormal) * 0.2f;
                        lastAngle = groundAngle;
                    }
                }
                
                rb.velocity = Vector3.ProjectOnPlane(rb.velocity, groundNormal);
                if (detachedTimer == 0)
                {
                    Vector3 lerped = Vector3.Lerp(rb.position, target, 25 * Time.fixedDeltaTime);
                    rb.position = lerped;
                }
            }
            else
            {
                groundNormal = Vector3.up;
                fsm.SetState<StateAir>();
            }
        }

        protected override void Move()
        {
            Vector3 velocity = rb.velocity;
            Math.SplitPlanarVector(velocity, groundNormal, out Vector3 planar, out Vector3 airVelocity);

            movementVector = planar;
            planarVelocity = planar;

            if (player.Flags.Check(Flag.AutoRun)) inputDir = transform.forward;
            
            if (inputDir.magnitude > 0.2f)
            {
                if (!braking)
                {
                    turnRate = Mathf.Lerp(turnRate, turnSpeed, Time.fixedDeltaTime * turnSmoothing);
                    accelRateMod = accelOverSpeed.Evaluate(planarVelocity.magnitude / topSpeed);
                    if (planarVelocity.magnitude < topSpeed)
                    {
                        planarVelocity += inputDir * (accelRate * accelRateMod * Time.fixedDeltaTime);
                    }
                
                    Vector3 newVelocity = Quaternion.FromToRotation(planarVelocity.normalized, inputDir.normalized) * planarVelocity;
                    float handling = turnRate * _groundHandlingAmount;
                    handling *= turnSpeedOverSpeed.Evaluate(planarVelocity.magnitude / topSpeed);
                    movementVector = Vector3.Slerp(planarVelocity, newVelocity, Time.fixedDeltaTime * handling);
                    
                    int roundedAngle = Mathf.RoundToInt(groundAngle);
                    bool decelOnTurn = roundedAngle is 0 or 90 or 180;
                    if (decelOnTurn)
                    {
                        float mod = config.turnDecelOverSpeed.Evaluate(movementVector.magnitude / topSpeed);
                        Vector3 horizontal = Vector3.ProjectOnPlane(rb.velocity.normalized, groundNormal);
                        float angle = Vector3.Angle(horizontal, movementVector) / 180 * config.turnDecel * mod;
                        movementVector -= movementVector * angle;
                    }
                }
                else
                {
                    Decel(20, 150);
                }
            }
            else
            {
                Decel(minDecelRate, maxDecelRate);
            }
            
            airVelocity = Vector3.ClampMagnitude(airVelocity, 125f);
            movementVector = Vector3.ClampMagnitude(movementVector, maxSpeed);
            Vector3 movementVelocity = movementVector + airVelocity;
            rb.velocity = movementVelocity;
        }

        protected virtual void Decel(float minDecelRate, float maxDecelRate)
        {
            if (!player.Flags.Check(Flag.DisableDeceleration))
            {
                float f = Mathf.Lerp(maxDecelRate, minDecelRate, movementVector.magnitude / topSpeed);
                if (movementVector.magnitude > 5f)
                {
                    movementVector = Vector3.MoveTowards(movementVector, Vector3.zero, Time.fixedDeltaTime * f);
                }
                else
                {
                    movementVector = Vector3.zero;
                }
            }
        }

        protected override void Rotate()
        {
            base.Rotate();
        }

        protected virtual void SlopePhysics()
        {
            groundAngle = Vector3.Angle(groundNormal, Vector3.up);
            if (rb.velocity.magnitude < stickSpeed && groundAngle >= 75)
            {
                detachedTimer = 0.15f;
                //rb.velocity = Vector3.zero;
                rb.AddForce(groundNormal * 0.15f, ForceMode.Impulse);
                groundNormal = Vector3.up;
                grounded = false;
            }
            
            float dot = Vector3.Dot(transform.up, Vector3.up);
            bool onWall = dot < 0.01f && dot > -0.01f && groundAngle >= 89 && groundAngle <= 91;
            
            if (groundAngle > minSlopeAngle && movementVector.magnitude > 3f)
            {
                bool uphill = Vector3.Dot(rb.velocity.normalized, vectorGravity) < 0;
                //AnimationCurve curve = InLoop && rb.velocity.magnitude > inLoopSpeedThreshold ? loopForceOverSpeed : slopeForceOverSpeed;
                float groundSpeedMod = slopeForceOverSpeed.Evaluate(rb.velocity.sqrMagnitude / topSpeed / topSpeed);
                Vector3 slopeForce = Vector3.ProjectOnPlane(vectorGravity, groundNormal) * (slopeMultiplier * (uphill ? groundSpeedMod : 1f));
                rb.velocity += slopeForce * Time.fixedDeltaTime;
            }
        }

        private void UpdateGroundNormal()
        {
            CheckGround(-_groundCheck.up, out var hit);

            groundNormal = hit.normal;
            transformNormal = groundNormal;

            if (fsm.LastState is not StateSpring)
            {
                // Vector3 target = hit.point + groundNormal * col.height / 2;
                // rb.position = target;
            }
        }

        #region CoolMath

        private void SlopePrediction(float dt)
        {
            float lowerValue = 0.43f;
            Vector3 predictedPosition = rb.position + -groundNormal * lowerValue;
            Vector3 predictedNormal = groundNormal;
            Vector3 predictedVelocity = rb.velocity;
            float speedFrame = rb.velocity.magnitude * dt;
            float lerpJump = 0.015f;

            Debug.DrawRay(predictedPosition, predictedVelocity.normalized * (speedFrame * 1.3f), Color.red, 5, true);
            if (!Physics.Raycast(predictedPosition, predictedVelocity.normalized, out RaycastHit pGround, speedFrame * 1.3f, _mask)) { HighSpeedFix(dt); return; }

            for (float lerp = lerpJump; lerp < 45 / 90; lerp += lerpJump)
            {
                Debug.DrawRay(predictedPosition, Vector3.Lerp(predictedVelocity.normalized, groundNormal, lerp) * (speedFrame * 1.3f), Color.blue, 5, false);
                if (!Physics.Raycast(predictedPosition, Vector3.Lerp(predictedVelocity.normalized, groundNormal, lerp), out pGround, speedFrame * 1.3f, _mask))
                {
                    lerp += lerpJump;
                    Debug.DrawRay(predictedPosition + Vector3.Lerp(predictedVelocity.normalized, groundNormal, lerp) * (speedFrame * 1.3f) + Vector3.right * 0.05f, -predictedNormal, Color.yellow, 5, false);
                    Physics.Raycast(predictedPosition + Vector3.Lerp(predictedVelocity.normalized, groundNormal, lerp) * (speedFrame * 1.3f), -predictedNormal, out pGround, _groundCheckDistance + 0.2f, _mask);

                    predictedPosition = predictedPosition + Vector3.Lerp(predictedVelocity.normalized, groundNormal, lerp) * speedFrame + pGround.normal * lowerValue;
                    predictedVelocity = Quaternion.FromToRotation(groundNormal, pGround.normal) * predictedVelocity;
                    groundNormal = pGround.normal;
                    rb.position = Vector3.MoveTowards(rb.position, predictedPosition, dt);
                    rb.velocity = predictedVelocity;
                    break;
                }
            }
        }
        
        private void HighSpeedFix(float dt)
        {
            Vector3 predictedPosition = rb.position;
            Vector3 predictedNormal = groundNormal;
            Vector3 predictedVelocity = rb.velocity;
            int steps = 8;
            int i;
            for (i = 0; i < steps; i++)
            {
                predictedPosition += predictedVelocity * dt / steps;
                if (Physics.Raycast(predictedPosition, -predictedNormal, out RaycastHit pGround, _groundCheckDistance + speedOverShoot, _mask))
                {
                    if (Vector3.Angle (predictedNormal, pGround.normal) < 45)
                    {
                        Debug.DrawRay(predictedPosition, -predictedNormal, Color.green);
                        predictedPosition = pGround.point + pGround.normal * 0.5f;
                        predictedVelocity = Quaternion.FromToRotation(groundNormal, pGround.normal) * predictedVelocity;
                        predictedNormal = pGround.normal;
                    } else
                    {
                        Debug.DrawRay(predictedPosition, -predictedNormal, Color.red);
                        i = -1;
                        break;
                    }
                } else
                {
                    Debug.DrawRay(predictedPosition, -predictedNormal, Color.red);
                    i = -1;
                    break;
                }
            }
            if (i >= steps)
            {
                groundNormal = predictedNormal;
                rb.position = Vector3.MoveTowards(rb.position, predictedPosition, dt);
            }
        }

        private void ConvertAirToGroundVelocity()
        {
            if (Physics.Raycast(gameObject.transform.position, rb.velocity.normalized, out RaycastHit velocityFix, rb.velocity.magnitude, _mask))
            {
                //Check if the angle is good
                float nextGroundAngle = Vector3.Angle(velocityFix.normal, Vector3.up);
                if (nextGroundAngle <= 20)
                {
                    Vector3 fixedVelocity = Vector3.ProjectOnPlane(rb.velocity, gameObject.transform.up);
                    fixedVelocity = Quaternion.FromToRotation(gameObject.transform.up, velocityFix.normal) * fixedVelocity;
                    rb.velocity = fixedVelocity;
                }
            }

        }

        #endregion

        public string SurfaceName()
        {
            if (currentSurface != null) return currentSurface.type.ToString();
            return "Concrete";
        }
    }
}
