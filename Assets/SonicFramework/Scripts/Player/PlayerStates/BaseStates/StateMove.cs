using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public abstract class StateMove : PlayerBaseState
    {
        public readonly Rigidbody rb;
        public Vector3 input;

        public Vector3 groundNormal;
        public Transform cameraTransform;

        public Vector3 planarVelocity;
        public Vector3 movementVector;
        public Vector3 inputDir;
        public Vector3 transformNormal;
        
        public float topSpeed = 45;
        public float maxSpeed;
        public float gravity;
        public Vector3 vectorGravity;
        public AnimationCurve turnSpeedOverSpeed;
        public AnimationCurve accelOverSpeed;
        public float turnRate = 8;
        public float turnSpeed = 8;
        public float turnSmoothing = 9;
        public float accelRate = 8;
        public float accelRateMod;
        public float minDecelRate = 8;
        public float maxDecelRate = 25;
        public float minSlopeAngle = 15;
        public int stickSpeed = 5;
        public float slopeMultiplier = 1.25f;
        public float inLoopSpeedThreshold = 15;
        public float speedOverShoot = 0.1f;
        public AnimationCurve slopeForceOverSpeed;
        public AnimationCurve loopForceOverSpeed;
        
        public Transform _groundCheck;
        public float _groundCheckDistance;
        public LayerMask _mask;
        public AnimationCurve _groundCheckOverSpeed;

        protected Transform closestRing;
        protected bool braking;

        public bool grounded;

        protected PlayerMovementConfig config;
        
        public StateMove(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject)
        {
            this.config = config;
            gravity = config.gravity;
            vectorGravity = new Vector3(0, config.gravity, 0);
            topSpeed = config.topSpeed;
            maxSpeed = config.maxSpeed;
            turnSpeedOverSpeed = config.turnSpeedOverSpeed;
            accelOverSpeed = config.accelOverSpeed;
            turnSpeed = config.turnSpeed;
            turnRate = config.turnRate;
            turnSmoothing = config.turnSmoothing;
            accelRate = config.accelRate;
            minDecelRate = config.minDecelRate;
            maxDecelRate = config.maxDecelRate;
            minSlopeAngle = config.minSlopeAngle;
            stickSpeed = config.stickSpeed;
            slopeMultiplier = config.slopeMultiplier;
            inLoopSpeedThreshold = config.inLoopSpeedThreshold;
            slopeForceOverSpeed = config.slopeForceOverSpeed;
            loopForceOverSpeed = config.loopForceOverSpeed;
            speedOverShoot = config.speedOverShoot;
            
            _groundCheck = gameObject.transform.Find("GroundCheck");
            _groundCheckDistance = config.groundCheckDistance;
            _groundCheckOverSpeed = config.groundCheckOverSpeed;
            _mask = config.groundMask;

            cameraTransform = Camera.main.transform; 
            rb = gameObject.GetComponent<Rigidbody>();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            CheckInput();
            CheckButtons();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Vector3 transformedInput = Quaternion.FromToRotation(cameraTransform.up, groundNormal) *
                                       (cameraTransform.rotation * new Vector3(input.x, 0f, input.y));
            transformedInput = Vector3.ProjectOnPlane(transformedInput, groundNormal);
            inputDir = transformedInput.normalized * input.magnitude;
        }

        public override void BaseUpdate()
        {
            topSpeed = config.topSpeed;
            maxSpeed = config.maxSpeed;
            gravity = config.gravity;
            turnSpeedOverSpeed = config.turnSpeedOverSpeed;
            accelOverSpeed = config.accelOverSpeed;
            turnSpeed = config.turnSpeed;
            turnRate = config.turnRate;
            turnSmoothing = config.turnSmoothing;
            accelRate = config.accelRate;
            minDecelRate = config.minDecelRate;
            maxDecelRate = config.maxDecelRate;
            minSlopeAngle = config.minSlopeAngle;
            stickSpeed = config.stickSpeed;
            slopeMultiplier = config.slopeMultiplier;
            inLoopSpeedThreshold = config.inLoopSpeedThreshold;
            slopeForceOverSpeed = config.slopeForceOverSpeed;
            loopForceOverSpeed = config.loopForceOverSpeed;
            speedOverShoot = config.speedOverShoot;
            _groundCheckDistance = config.groundCheckDistance;
            _groundCheckOverSpeed = config.groundCheckOverSpeed;
            _mask = config.groundMask;
            
            float skiddingDot = Vector3.Dot(inputDir, rb.velocity.normalized);
            float brakeThreshold = 0.65f;
            braking = skiddingDot < -brakeThreshold;
            animator.SetBool("Braking", braking);
        }

        protected virtual void CheckInput()
        {
            input = player.Input.movementInput;
        }

        protected virtual void CheckButtons()
        {
            
        }

        protected abstract void Move();
        
        protected virtual void Rotate()
        {
            transformNormal = Vector3.Slerp(transformNormal, groundNormal, Time.fixedDeltaTime * 4);
            
            if (!Math.IsApproximate(rb.velocity, Vector3.zero, 0.2f))
            {
                Vector3 lookDir = transform.forward;
                lookDir = Vector3.Slerp(transform.forward, Vector3.ProjectOnPlane(rb.velocity, transformNormal).normalized, Time.fixedDeltaTime * 16f);
                Quaternion rotation = Quaternion.LookRotation(lookDir, transformNormal);
                transform.rotation = rotation;
            }
        }

        protected bool CheckGround(Vector3 dir, out RaycastHit info)
        {
            float gM = _groundCheckOverSpeed.Evaluate(rb.velocity.magnitude / topSpeed);
            float g = _groundCheckDistance * gM;
            
            bool result = Physics.Raycast(_groundCheck.position, dir, out var ground, g, _mask,
                QueryTriggerInteraction.Ignore);
            info = ground;

            return result;
        }
        
        protected virtual bool PredictSpring()
        {
            return Physics.Raycast(gameObject.transform.position, -gameObject.transform.up,
                Mathf.Max(1.1f, rb.velocity.magnitude * 0.2f), LayerMask.NameToLayer("Homing"));
        }

        protected virtual Transform GetNearestRing()
        {
            Vector3 origin = transform.position;

            Collider[] ringsInRange = Physics.OverlapSphere(origin + Vector3.Cross(transform.right, Vector3.up) * 0.2f, config.lsdFindFirstRadius, config.ringMask);
            
            Transform closestRing = null;
            float distance = 1f;
            foreach (Collider r in ringsInRange)
            {
                Transform target = r.transform;
                Vector3 direction = target.position - origin;
                float targetDistance = (direction.sqrMagnitude / config.lsdFindFirstRadius) / config.lsdFindFirstRadius;
                if (targetDistance < distance)
                {
                    if (!Physics.Linecast(origin, target.position, _mask))
                    {
                        closestRing = target;
                        distance = targetDistance;
                    }
                }
            }

            return closestRing;
        }
    }
}
