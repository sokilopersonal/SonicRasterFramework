using SonicFramework.PlayerStates;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class DefaultCameraState : CameraBaseState
    {
        protected float x;
        protected float y;
        private float dynamicDistance;
        private float dynamicFov;
        private float leanThreshold;
        private float distance;
        private float collisionResult;
        private Vector3 rotationVector;
        private Vector3 normal;
        private Vector3 velocity;
        private Vector3 leanOffset;
        private Vector3 verticalLeanOffset;

        private float idleTimer;
        private float idleThreshold;
        private float tilt;
        
        public DefaultCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
            idleThreshold = 5f;
            leanThreshold = config.leanThreshold;
            
            SetDir(player.transform.forward);
        }

        public override void Enter()
        {
            idleTimer = 0;
            
            player.fsm.OnStateChanged += OnStateChanged;
        }

        public override void Exit()
        {
            player.fsm.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State obj)
        {
            fsm.GetState<RecoveryCameraState>(out var recovery);
            
            if (obj is StateRoll && player.fsm.LastState is StateDropDashForce)
            {
                recovery.data = new RecoveryData(0.3f);
                
                fsm.SetState<DelayedCameraState>();
            }

            if (obj is StateRoll && player.fsm.LastState is StateSpinDashCharge)
            {
                recovery.data = new RecoveryData(0.75f);
                
                fsm.SetState<DelayedCameraState>();
            }
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            Actions();
            CountIdle();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                fsm.GetState<PhotoCameraState>().Set(x, y);
                fsm.SetState<PhotoCameraState>();
            }
        }

        private void Actions()
        {
            InputThings();
            Alignment();
            Leaning();
            Dynamic();
            Positioning();
            Rotation();
            Collision();
        }

        protected virtual void InputThings()
        {
            x += input.x * sens;
            y -= input.y * sens;
            y = Mathf.Clamp(y, -75, 75);

            if (input.magnitude > 0.1f) idleTimer = 0;

            rotationVector = new Vector3(y, x, 0);
        }

        protected virtual void CountIdle()
        {
            // if (input.magnitude < 0.1f)
            // {
            //     idleTimer += Time.deltaTime;
            // }
            //
            // if (idleTimer >= idleThreshold)
            // {
            //     fsm.SetState<AutoCameraState>();
            // }
        }

        protected virtual void Dynamic()
        {
            dynamicDistance =
                Mathf.Lerp(minDistance, maxDistance, player.fsm.Velocity.magnitude / player.Config.topSpeed / 2.5f);
            distance = Mathf.Lerp(distance, dynamicDistance, 12 * Time.deltaTime);

            dynamicFov = player.fsm.CurrentState is not StateSpinDashCharge ? 
                Mathf.Lerp(minFov, maxFov, player.fsm.Velocity.magnitude / player.Config.topSpeed / 2.5f) :
                Mathf.Lerp(dynamicFov, 50, 1.1f * Time.deltaTime);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, dynamicFov, 12 * Time.deltaTime);
        }

        protected virtual void Alignment()
        {
            if (settings.slopeAlignment)
            {
                var current = player.fsm.CurrentState;

                if (current is StateGround state)
                {
                    normal = state.groundNormal;
                }

                if (current is StateSpinDashCharge state2)
                {
                    normal = state2.groundNormal;
                }
                
                if (current is StateAir)
                {
                    normal = Vector3.up;
                }
            }
            else
            {
                normal = Vector3.up;
            }
            
            Quaternion from = Quaternion.FromToRotation(holder.up, normal) * holder.rotation;
            holder.rotation = Quaternion.Slerp(holder.rotation, from, transitionSpeed * Time.deltaTime);
        }

        protected virtual void Positioning()
        {
            Vector3 holderTarget = player.transform.position;
            Vector3 holderDamp = Vector3.Lerp(holder.position, holderTarget, config.smoothTime * Time.deltaTime);
            holder.position = holderTarget;
            
            Vector3 target = Quaternion.Euler(rotationVector) * new Vector3(0, 0, -distance);
            Vector3 damp = Vector3.Lerp(camTransform.position, target, config.smoothTime * Time.deltaTime);
            camTransform.localPosition = target;
        }

        protected virtual void Rotation()
        {
            Quaternion result = Quaternion.Lerp(camTransform.rotation, 
                Quaternion.LookRotation(gameObject.transform.position - camTransform.position, normal),
                translationSpeed * Time.deltaTime);
            camTransform.rotation = result;
        }

        protected virtual void Collision()
        {
            // float result = Physics.SphereCast(gameObject.transform.position, radius, -camTransform.forward, out RaycastHit hit, distance, mask, QueryTriggerInteraction.Ignore)
            //      ? hit.distance - radius
            //      : distance;
            //  collisionResult = Mathf.Lerp(collisionResult, result, collisionLerpSpeed * Time.deltaTime);
            
            float result = Physics.Linecast(gameObject.transform.position, camTransform.position, out RaycastHit hit, mask, QueryTriggerInteraction.Ignore)
                 ? hit.distance
                 : distance;
            collisionResult = result;

             var dirOffset = camTransform.TransformDirection(lookOffset + leanOffset + verticalLeanOffset);
             //var dirOffset = Vector3.zero;
             camTransform.position = gameObject.transform.position + dirOffset - camTransform.forward * collisionResult;
        }

        protected virtual void Leaning()
        {
            if (settings.enableCameraLeaning)
            {
                GroundLean();

                if (player.fsm.CurrentState is StateAir)
                {
                    float value = Mathf.Clamp(-player.fsm.Velocity.y * 0.05f, -1f, 1.1f);
                    float y = Mathf.Lerp(verticalLeanOffset.y, value, 4 * Time.deltaTime);

                    verticalLeanOffset.y = y;
                }
                else if (player.fsm.CurrentState is StateGround)
                {
                    float value = Mathf.Clamp(-player.fsm.Velocity.y * 0.01f, -1f, 0.5f);
                    float y = Mathf.Lerp(verticalLeanOffset.y, value, 4 * Time.deltaTime);

                    verticalLeanOffset.y = y;
                }
                else
                {
                    verticalLeanOffset = Vector3.Lerp(verticalLeanOffset, Vector3.zero, 5f * Time.deltaTime);
                }
            }
            else
            {
                leanOffset = Vector3.zero;
            }

            void Reset()
            {
                leanOffset = Vector3.Lerp(leanOffset, Vector3.zero, velocitySpeed * 0.75f * Time.deltaTime);
            }

            void GroundLean()
            {
                if (player.fsm.CurrentState is StateGround)
                {
                    if (player.fsm.Velocity.magnitude > 20f)
                    {
                        float x = player.fsm.LocalVelocity.x;

                        Vector3 target = new Vector3(Mathf.Clamp(x, -config.maxLean, config.maxLean), 0, 0);

                        if (Mathf.Abs(target.x) >= leanThreshold - config.maxLean * 0.2f)
                        {
                            leanOffset = Vector3.Lerp(leanOffset, target, velocitySpeed * Time.deltaTime);
                        }
                        else
                        {
                            Reset();
                        }
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    Reset();
                }
            }
        }

        public void Set(params float[] el)
        {
            x = el[0];
            y = el[1];
        }

        public void SetDir(Vector3 dir)
        {
            var result = Quaternion.LookRotation(dir, Vector3.up).eulerAngles;
            
            Set(result.y, y);
        }
    }
}