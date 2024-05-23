using UnityEngine;

namespace SonicFramework
{
    [SelectionBase, RequireComponent(typeof(Rigidbody))]
    public class PlayerPhysics : PlayerComponent
    {
        // public Rigidbody rb { get; private set; }
        //
        // [Header("Braking")] 
        // [SerializeField] private float brakingInputThreshold = 1f;
        // [SerializeField] private float maxBrakeRate = 45f;
        // [SerializeField] private float minBrakeRate = 12f;
        //
        // [Header("Jump Dash")] 
        // [SerializeField] private float jumpDashForce = 2f;
        // [SerializeField] private float jumpDashTimer = 0.45f;
        // [SerializeField] private float jumpDashMaxSpeed = 65f;
        // private bool jumpDashed;
        // private float jumpDashCycleTimer;
        // private Coroutine dashCoroutine;
        //
        // [Header("Homing Attack")]
        // public Transform currentHomingTarget;
        // [SerializeField] private float homingSpeed;
        // [SerializeField] private float homingFindRadius = 15f;
        // [SerializeField] private float homingTimer;
        // [SerializeField] private LayerMask homingMask;
        // private bool inHoming;
        //
        // [Header("Roll")]
        // [SerializeField] private float rollMultiplier;
        // [SerializeField] private float rollSteering;
        // private bool isRolling;
        // private bool canRoll = true;
        //
        // [Header("Drop Dash")]
        // [SerializeField] private float dropDashPower;
        // [SerializeField] private float maxDropDashPower = 1.8f;
        // private bool waitingForTheDropDash;
        // private bool canDropDash = true;
        // private Vector3 dropDashStart;
        //
        // [Header("Light Speed Dash")] 
        // [SerializeField] private float searchRadius;
        // [SerializeField] private float lightSpeedDashForce;
        // [SerializeField] private LayerMask lightSpeedDashMask;
        // private float lightSpeedDashTimer;
        // private SplineContainer splineContainer;
        // private SplineAnimate splineAnimate;
        // private bool inLightSpeedDash;
        // private bool reverseLightSpeedDash;
        // private Vector3 nearestPoint;
        //
        // private bool grounded;
        // private bool braking;
        // private bool useGravity = true;
        // private bool onSlope;
        //
        // [HideInInspector] public bool onSpring;
        // public bool MovementLocked { get; set; }
        // public float groundAngle { get; private set; }
        // [HideInInspector] public Vector3 groundNormal;
        // [HideInInspector] public bool inLoop;
        // private RaycastHit hit;
        // public Vector3 MovementInput { get; set; }
        // public Vector3 Velocity
        // {
        //     get => rb.velocity;
        //     set
        //     {
        //         rb.velocity = value;
        //     }
        // }
        //
        //
        // private Transform cameraTransform;
        // private RaycastHit slopeHit;
        //
        // private Surface currentSurface;
        // private Surface previousSurface;
        //
        // public event Action OnPlayerGrounded;
        //
        // public event Action OnPlayerJumped;
        // public event Action OnPlayerJumpDashed;
        //
        // public event Action OnPlayerHomingStarted;
        // public event Action OnPlayerHomingEnded;
        //
        // public event Action OnPlayerSlideStarted;
        // public event Action OnPlayerSlideEnded;
        //
        // public event Action OnPlayerDropDashStarted;
        // public event Action OnPlayerDropDashEnded;
        //
        // [HideInInspector] public float timer;
        // public bool showDebugInfo;
        //
        // private void Awake()
        // {
        //     rb = GetComponent<Rigidbody>();
        //     
        //     cameraTransform = Camera.main.transform;
        // }
        //
        // private void Update()
        // {
        //     InputActions();
        //     CountTimer();
        //
        //     // splineContainer = FindClosestSplineContainer();
        //     //
        //     // if (inLightSpeedDash)
        //     // {
        //     //     var length = splineContainer.Spline.GetLength();
        //     //     lightSpeedDashTimer =
        //     //         Mathf.Clamp(lightSpeedDashTimer + lightSpeedDashForce * Time.deltaTime, 0, length);
        //     //     var ourPosWorld = transform.position;
        //     //     SplineUtility.GetNearestPoint(splineContainer.Spline,
        //     //         splineContainer.transform.InverseTransformPoint(ourPosWorld), out var nearestPointLocal, out var n, 32, 12);
        //     //     splineContainer.Spline.Evaluate(n, out var pos, out var dir, out var _);
        //     //     nearestPoint = splineContainer.transform.TransformPoint(nearestPointLocal);
        //     //     
        //     //     transform.rotation = Quaternion.LookRotation(Math.ConvertFloat3ToVector3(dir), transform.up);
        //     //
        //     //     if (n >= length)
        //     //     {
        //     //         rb.velocity = Math.ConvertFloat3ToVector3(dir).normalized * lightSpeedDashForce;
        //     //         
        //     //         inLightSpeedDash = false;
        //     //         useGravity = true;
        //     //         MovementLocked = false;
        //     //         
        //     //         Destroy(splineContainer);
        //     //     }
        //     // }
        // }
        //
        // private void InputActions()
        // {
        //     if (!MovementLocked)
        //     {
        //         MovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //
        //         if (Input.GetKeyDown(KeyCode.Space))
        //         {
        //             Jump();
        //         }
        //
        //         if (Input.GetKeyDown(KeyCode.F))
        //         {
        //             if (!grounded && canDropDash)
        //             {
        //                 waitingForTheDropDash = true;
        //                 dropDashStart = transform.position;
        //                 rb.AddForce(Vector3.up * 0.35f, ForceMode.Impulse);
        //                 OnPlayerDropDashStarted?.Invoke();
        //                 canDropDash = false;
        //             }
        //
        //             if (rb.velocity.magnitude > 3 && grounded)
        //             {
        //                 isRolling = true;
        //
        //                 OnPlayerSlideStarted?.Invoke();
        //             }
        //         }
        //         else if (Input.GetKeyUp(KeyCode.F))
        //         {
        //             if (grounded)
        //             {
        //                 canDropDash = true;
        //             }
        //
        //             waitingForTheDropDash = false;
        //
        //             isRolling = false;
        //
        //             OnPlayerSlideEnded?.Invoke();
        //         }
        //         
        //         if (Input.GetKeyDown(KeyCode.Z))
        //         {
        //             if (splineContainer)
        //             {
        //                 var length = splineContainer.Spline.GetLength();
        //                 lightSpeedDashTimer =
        //                     Mathf.Clamp(lightSpeedDashTimer + lightSpeedDashForce * Time.deltaTime, 0, length);
        //                 var ourPosWorld = transform.position;
        //                 SplineUtility.GetNearestPoint(splineContainer.Spline,
        //                     splineContainer.transform.InverseTransformPoint(ourPosWorld), out var nearestPointLocal, out var n, 32, 12);
        //                 splineContainer.Spline.Evaluate(n, out var pos, out var dir, out var _);
        //                 nearestPoint = splineContainer.transform.TransformPoint(nearestPointLocal);
        //                 
        //                 lightSpeedDashTimer = 0;
        //                 
        //                 rb.velocity = Vector3.zero;
        //                 rb.position = splineContainer.transform.TransformPoint(Math.ConvertFloat3ToVector3(nearestPoint));
        //                 
        //                 inLightSpeedDash = true;
        //                 useGravity = false;
        //
        //                 MovementLocked = true;
        //             }
        //         }
        //     }
        // }
        //
        // private void FixedUpdate()
        // {
        //     float GroundCheckMod = groundCheckOverSpeed.Evaluate(rb.velocity.magnitude / TopSpeed);
        //     float GroundCheck = groundCheckDistance * (grounded ? GroundCheckMod : 1);
        //     
        //     if (Physics.Raycast(groundCheck.position, -groundCheck.up, out RaycastHit ground, GroundCheck, mask, QueryTriggerInteraction.Ignore))
        //     {
        //         if (!grounded)
        //         {
        //             OnJumpDashEnd();
        //             OnPlayerGrounded?.Invoke();
        //             canDropDash = true;
        //             onSpring = false;
        //             grounded = true;
        //         }
        //         
        //         if (currentSurface != null)
        //         {
        //             ground.transform.TryGetComponent(out Surface surface);
        //             
        //             if (currentSurface != surface)
        //             {
        //                 currentSurface = surface;
        //             }
        //         }
        //         else
        //         {
        //             ground.transform.TryGetComponent(out Surface surface);
        //             
        //             currentSurface = surface;
        //         }
        //         
        //         if (waitingForTheDropDash)
        //         {
        //             Vector3 diff = transform.position - dropDashStart;
        //             diff.x = 0;
        //             diff.z = 0;
        //
        //             float p = Mathf.Clamp(Mathf.Max(dropDashPower, diff.magnitude / 28), dropDashPower, maxDropDashPower);
        //             
        //             if (rb.velocity.magnitude < 85)
        //             {
        //                 Vector3 dir = transform.forward;
        //                 rb.AddForce(dir * p, ForceMode.Impulse);
        //             }
        //             
        //             waitingForTheDropDash = false;
        //             canDropDash = true;
        //             OnPlayerDropDashEnded?.Invoke();
        //
        //             if (!MovementLocked)
        //             {
        //                 if (Input.GetKey(KeyCode.F))
        //                 {
        //                     isRolling = true;
        //                 }
        //             }
        //         }
        //         
        //         groundNormal = ground.normal;
        //         TransformNormal = Vector3.Slerp(TransformNormal, groundNormal, Time.fixedDeltaTime * 20f);
        //         
        //         Slide();
        //         Movement();
        //         
        //         if (!jumped && !inLightSpeedDash && !onSpring)
        //         {
        //             rb.velocity = Vector3.ProjectOnPlane(rb.velocity, groundNormal);
        //             Vector3 target = ground.point + ground.normal * player.Model.col.height / 2;
        //             rb.position = target;
        //         }
        //         
        //         if (rb.velocity.magnitude > stickSpeed)
        //         {
        //             SlopePrediction(Time.fixedDeltaTime);
        //         }
        //         
        //         SlopePhysics();
        //         
        //         groundAngle = Vector3.Angle(groundNormal, Vector3.up);
        //
        //         currentHomingTarget = null;
        //     }
        //     else
        //     {
        //         grounded = false;
        //         if (braking) braking = false;
        //         groundNormal = -vectorGravity.normalized;
        //         Movement();
        //         if (useGravity)
        //         {
        //             rb.velocity += vectorGravity * Time.fixedDeltaTime;
        //         }
        //         TransformNormal = Vector3.Slerp(TransformNormal, Vector3.up, Time.fixedDeltaTime * 12f);
        //         SnapToWall();
        //
        //         currentHomingTarget = FindClosestHomingTarget();
        //
        //         if (currentHomingTarget != null)
        //         {
        //             if (!HomingTargetInSight(currentHomingTarget))
        //             {
        //                 currentHomingTarget = null;
        //             }
        //         }
        //     }
        //     
        //     Rotate();
        // }
        //
        // private void Movement()
        // {
        //     Vector3 velocity = rb.velocity;
        //     Math.SplitPlanarVector(velocity, groundNormal, out Vector3 planar, out Vector3 AirVelocity);
        //
        //     MovementVector = planar;
        //     PlanarVelocity = planar;
        //
        //     Vector3 TransformedInput = Quaternion.FromToRotation(cameraTransform.up, groundNormal) *
        //                                (cameraTransform.rotation * MovementInput);
        //     TransformedInput = Vector3.ProjectOnPlane(TransformedInput, groundNormal);
        //     InputDir = TransformedInput.normalized * MovementInput.magnitude;
        //     
        //     if (!braking)
        //     {
        //         if (InputDir.magnitude > 0.2f)
        //         {
        //             if (!isRolling)
        //             {
        //                 turnRate = Mathf.Lerp(turnRate, turnSpeed, Time.fixedDeltaTime * turnSmoothing);
        //                 if (PlanarVelocity.magnitude < TopSpeed)
        //                     PlanarVelocity += InputDir * (accelRate * Time.fixedDeltaTime);
        //                 float Handling = turnRate * (grounded ? groundHandlingAmount : airHandlingAmount);
        //                 Handling *= turnSpeedOverSpeed.Evaluate(PlanarVelocity.magnitude / TopSpeed);
        //                 MovementVector = Vector3.Lerp(PlanarVelocity, InputDir.normalized * PlanarVelocity.magnitude, Time.fixedDeltaTime * Handling);
        //             }
        //             else
        //             {
        //                 turnRate = Mathf.Lerp(turnRate, rollSteering, Time.fixedDeltaTime * turnSmoothing);
        //                 Vector3 NewVelocity = Quaternion.FromToRotation(PlanarVelocity.normalized, InputDir.normalized) * PlanarVelocity;
        //                 float Handling = turnRate * (grounded ? groundHandlingAmount : airHandlingAmount);
        //                 Handling *= turnSpeedOverSpeed.Evaluate(PlanarVelocity.magnitude / TopSpeed);
        //                 MovementVector = Vector3.Slerp(PlanarVelocity, NewVelocity, Time.fixedDeltaTime * Handling);
        //             }
        //         }
        //         else
        //         {
        //             if (!isRolling)
        //             {
        //                 float decel = Mathf.Lerp(maxDecelRate, minDecelRate, MovementVector.magnitude / TopSpeed);
        //                 if (MovementVector.magnitude > 1f)
        //                     MovementVector = Vector3.MoveTowards(MovementVector, Vector3.zero, decel * Time.fixedDeltaTime);
        //                 else
        //                 {
        //                     MovementVector = Vector3.zero;
        //                 }
        //             }
        //         }
        //     }
        //     else
        //     {
        //         float decel = Mathf.Lerp(maxBrakeRate, minBrakeRate, MovementVector.magnitude / TopSpeed);
        //         if (MovementVector.magnitude > 2f)
        //             MovementVector = Vector3.MoveTowards(MovementVector, Vector3.zero, decel * Time.fixedDeltaTime);
        //         else
        //             MovementVector = Vector3.zero;
        //     }
        //
        //     if (!MovementLocked)
        //     {
        //         if (grounded)
        //         {
        //             if (Vector3.Dot(InputDir, rb.velocity.normalized) < -brakingInputThreshold && !braking)
        //             {
        //                 braking = true;
        //             }
        //
        //             if (braking && Vector3.Dot(InputDir, rb.velocity.normalized) > brakingInputThreshold ||
        //                 MovementVector.sqrMagnitude < 0.1f)
        //             {
        //                 braking = false;
        //             }
        //
        //             // if (Velocity.magnitude < fullBrakeThreshold && MovementInput == Vector3.zero && grounded)
        //             // {
        //             //     MovementVector = Vector3.zero;
        //             //     rb.constraints = RigidbodyConstraints.FreezeAll;
        //             // }
        //             // else rb.constraints = RigidbodyConstraints.FreezeRotation;
        //         }
        //     }
        //
        //     AirVelocity = Vector3.ClampMagnitude(AirVelocity, 125f);
        //     Vector3 movementVelocity = MovementVector + AirVelocity;
        //     rb.velocity = movementVelocity;
        // }
        //
        // private void Jump()
        // {
        //     if (!grounded && !jumpDashed && currentHomingTarget == null)
        //     {
        //         if (dashCoroutine != null) StopCoroutine(dashCoroutine);
        //         Vector3 dir = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        //         dashCoroutine = StartCoroutine(JumpDash(dir, jumpDashTimer, jumpDashForce, jumpDashMaxSpeed));
        //         jumpDashed = true;
        //         jumped = true;
        //         OnPlayerJumpDashed?.Invoke();
        //         return;
        //     }
        //
        //     if (!grounded && currentHomingTarget != null)
        //     {
        //         if (dashCoroutine != null) StopCoroutine(dashCoroutine);
        //         jumpDashCycleTimer = 0;
        //         jumpDashed = true;
        //         inHoming = true;
        //         Vector3 dir = (currentHomingTarget.transform.position - transform.position).normalized;
        //         Lock();
        //         dashCoroutine = StartCoroutine(JumpDash(dir, homingTimer, homingSpeed, 100, false));
        //         OnPlayerHomingStarted?.Invoke();
        //     }
        //     
        //     if (!grounded) return;
        //
        //     grounded = false;
        //     jumped = true;
        //     Vector3 jumpDir = groundNormal + Vector3.up * 0.5f;
        //     rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
        //     OnPlayerJumped?.Invoke();
        // }
        //
        // private void Slide()
        // {
        //     if (isRolling)
        //     {
        //         player.Model.col.height = 0.2f;
        //     }
        //     else
        //     {
        //         player.Model.col.height = 1f;
        //     }
        // }
        //
        // private void SlopePhysics()
        // {
        //     groundAngle = Vector3.Angle(groundNormal, Vector3.up);
        //     if (rb.velocity.magnitude < stickSpeed && groundAngle > 55)
        //     {
        //         rb.velocity = Vector3.zero;
        //         rb.position += groundNormal * 0.5f;
        //         rb.AddForce(groundNormal * 0.15f, ForceMode.Impulse);
        //         groundNormal = Vector3.up;
        //         isRolling = false;
        //         grounded = false;
        //     }
        //     
        //     if (groundAngle > minSlopeAngle)
        //     {
        //         if (!braking)
        //         {
        //             bool uphill = Vector3.Dot(rb.velocity.normalized, vectorGravity) < 0;
        //             AnimationCurve curve = inLoop && rb.velocity.magnitude > inLoopSpeedThreshold ? loopForceOverSpeed : slopeForceOverSpeed;
        //             float groundSpeedMod = curve.Evaluate(rb.velocity.sqrMagnitude / TopSpeed / TopSpeed);
        //             float rollingMod = isRolling ? rollMultiplier : slopeMultiplier;
        //             Vector3 slopeForce = Vector3.ProjectOnPlane(vectorGravity, groundNormal) * (rollingMod * (uphill ? groundSpeedMod : 1f));
        //             rb.velocity += slopeForce * Time.fixedDeltaTime;
        //         }
        //     }
        // }
        //
        // private void Rotate()
        // {
        //     TransformNormal = Vector3.Slerp(TransformNormal, groundNormal, Time.fixedDeltaTime * 16f);
        //     
        //     if (!Math.IsApproximate(rb.velocity, Vector3.zero, 0.25f))
        //     {
        //         Vector3 lookDir = transform.forward;
        //         lookDir = Vector3.Slerp(transform.forward, Vector3.ProjectOnPlane(rb.velocity, groundNormal).normalized, Time.deltaTime * 25f);
        //         Quaternion rotation = Quaternion.LookRotation(lookDir, TransformNormal);
        //         transform.rotation = rotation;
        //     }
        // }
        //
        // public void CountTimer()
        // {
        //     if (timer > 0)
        //     {
        //         timer -= Time.deltaTime;
        //         
        //         Lock();
        //         player.Camera.Lock(true);
        //     }
        //     
        //     if (timer <= 0)
        //     {
        //         Unlock();
        //         player.Camera.Lock(false);
        //         
        //         timer = 0;
        //     }
        // }
        //
        // private IEnumerator JumpDash(Vector3 dir, float jumpDashTime, float jumpDashForce, float maxSpeed, bool lockY = true)
        // {
        //     rb.velocity = Vector3.zero;
        //
        //     Vector3 force = dir * jumpDashForce;
        //     rb.velocity = force;
        //     
        //     Lock();
        //     
        //     while (jumpDashCycleTimer < jumpDashTime)
        //     {
        //         if (lockY)
        //         {
        //             rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //         }
        //         
        //         if (rb.velocity.magnitude > maxSpeed)
        //         {
        //             rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        //         }
        //         
        //         jumpDashCycleTimer += Time.deltaTime;
        //         yield return null;
        //     }
        //     
        //     OnJumpDashEnd();
        // }
        //
        // private void OnJumpDashEnd()
        // {
        //     Unlock();
        //     inHoming = false;
        // }
        //
        // private void SnapToWall()
        // {
        //     if (rb.velocity.magnitude > 20)
        //     {
        //         if (CheckWall(out Vector3 normal))
        //         {
        //             if (dashCoroutine != null) StopCoroutine(dashCoroutine);
        //             OnJumpDashEnd();
        //             groundNormal = normal;
        //             jumped = false;
        //             jumpDashed = false;
        //         }
        //     }
        // }
        //
        // private bool CheckWall(out Vector3 normal)
        // {
        //     if (Physics.Raycast(transform.position + Vector3.up / 2, -transform.right, out var hitInfo1, groundCheckDistance / 2,
        //             mask, QueryTriggerInteraction.Ignore))
        //     {
        //         if (!hitInfo1.transform.CompareTag("ClimbableWall"))
        //         {
        //             normal = Vector3.up;
        //             return false;
        //         }
        //         
        //         normal = hitInfo1.normal;
        //         return true;
        //     }
        //
        //     if (Physics.Raycast(transform.position + Vector3.up / 2, transform.right, out var hitInfo2, groundCheckDistance / 2, 
        //             mask, QueryTriggerInteraction.Ignore))
        //     {
        //         if (!hitInfo2.transform.CompareTag("ClimbableWall"))
        //         {
        //             normal = Vector3.up;
        //             return false;
        //         }
        //         
        //         normal = hitInfo2.normal;
        //         return true;
        //     }
        //
        //     normal = Vector3.up;
        //     return false;
        // }
        //
        // private void SlopePrediction(float dt)
        // {
        //     float LowerValue = 0.43f;
        //     Vector3 PredictedPosition = rb.position + -groundNormal * LowerValue;
        //     Vector3 PredictedNormal = groundNormal;
        //     Vector3 PredictedVelocity = rb.velocity;
        //     float SpeedFrame = rb.velocity.magnitude * dt;
        //     float LerpJump = 0.015f;
        //
        //     Debug.DrawRay(PredictedPosition, PredictedVelocity.normalized * (SpeedFrame * 1.3f), Color.red, 5, true);
        //     if (!Physics.Raycast(PredictedPosition, PredictedVelocity.normalized, out RaycastHit pGround, SpeedFrame * 1.3f, mask)) { HighSpeedFix(dt); return; }
        //
        //     for (float lerp = LerpJump; lerp < 45 / 90; lerp += LerpJump)
        //     {
        //         Debug.DrawRay(PredictedPosition, Vector3.Lerp(PredictedVelocity.normalized, groundNormal, lerp) * (SpeedFrame * 1.3f), Color.blue, 5, false);
        //         if (!Physics.Raycast(PredictedPosition, Vector3.Lerp(PredictedVelocity.normalized, groundNormal, lerp), out pGround, SpeedFrame * 1.3f, mask))
        //         {
        //             lerp += LerpJump;
        //             Debug.DrawRay(PredictedPosition + Vector3.Lerp(PredictedVelocity.normalized, groundNormal, lerp) * (SpeedFrame * 1.3f) + Vector3.right * 0.05f, -PredictedNormal, Color.yellow, 5, false);
        //             Physics.Raycast(PredictedPosition + Vector3.Lerp(PredictedVelocity.normalized, groundNormal, lerp) * (SpeedFrame * 1.3f), -PredictedNormal, out pGround, groundCheckDistance + 0.2f, mask);
        //
        //             PredictedPosition = PredictedPosition + Vector3.Lerp(PredictedVelocity.normalized, groundNormal, lerp) * SpeedFrame + pGround.normal * LowerValue;
        //             PredictedVelocity = Quaternion.FromToRotation(groundNormal, pGround.normal) * PredictedVelocity;
        //             groundNormal = pGround.normal;
        //             rb.position = Vector3.MoveTowards(rb.position, PredictedPosition, dt);
        //             rb.velocity = PredictedVelocity;
        //             break;
        //         }
        //     }
        // }
        //
        // private void HighSpeedFix(float dt)
        // {
        //     Vector3 PredictedPosition = rb.position;
        //     Vector3 PredictedNormal = groundNormal;
        //     Vector3 PredictedVelocity = rb.velocity;
        //     int steps = 8;
        //     int i;
        //     for (i = 0; i < steps; i++)
        //     {
        //         PredictedPosition += PredictedVelocity * dt / steps;
        //         if (Physics.Raycast(PredictedPosition, -PredictedNormal, out RaycastHit pGround, groundCheckDistance + speedOverShoot, mask))
        //         {
        //             if (Vector3.Angle (PredictedNormal, pGround.normal) < 45)
        //             {
        //                 Debug.DrawRay(PredictedPosition, -PredictedNormal, Color.green);
        //                 PredictedPosition = pGround.point + pGround.normal * 0.5f;
        //                 PredictedVelocity = Quaternion.FromToRotation(groundNormal, pGround.normal) * PredictedVelocity;
        //                 PredictedNormal = pGround.normal;
        //             } else
        //             {
        //                 Debug.DrawRay(PredictedPosition, -PredictedNormal, Color.red);
        //                 i = -1;
        //                 break;
        //             }
        //         } else
        //         {
        //             Debug.DrawRay(PredictedPosition, -PredictedNormal, Color.red);
        //             i = -1;
        //             break;
        //         }
        //     }
        //     if (i >= steps)
        //     {
        //         groundNormal = PredictedNormal;
        //         rb.position = Vector3.MoveTowards(rb.position, PredictedPosition, dt);
        //     }
        // }
        //
        // private Transform FindClosestHomingTarget()
        // {
        //     Collider[] colliders = Physics.OverlapSphere(transform.position, homingFindRadius, homingMask);
        //     Transform closestObject = null;
        //     float closestDistance = Mathf.Infinity;
        //
        //     foreach (Collider collider in colliders)
        //     {
        //         float distance = Vector3.Distance(transform.position, collider.transform.position);
        //         if (distance < closestDistance)
        //         {
        //             closestDistance = distance;
        //             closestObject = collider.transform;
        //         }
        //     }
        //
        //     return closestObject;
        // }
        //
        // private SplineContainer FindClosestSplineContainer()
        // {
        //     Collider[] lightDashSplines = Physics.OverlapSphere(transform.position, searchRadius, lightSpeedDashMask);
        //     Transform closestObject = null;
        //     float closestDistance = Mathf.Infinity;
        //
        //     foreach (Collider collider in lightDashSplines)
        //     {
        //         float distance = Vector3.Distance(transform.position, collider.transform.position);
        //         if (distance < closestDistance)
        //         {
        //             closestDistance = distance;
        //             closestObject = collider.transform;
        //         }
        //     }
        //
        //     SplineContainer container = null;
        //     closestObject?.parent.TryGetComponent(out container);
        //     return container;
        // }
        //
        //
        // // Public functions
        //
        // public void Lock()
        // {
        //     MovementLocked = true;
        //     MovementInput = Vector3.zero;
        //     useGravity = false;
        // }
        //
        // public void Unlock()
        // {
        //     MovementLocked = false;
        //     useGravity = true;
        // }
        //
        // public Vector3 GroundVelocity() => Velocity;
        // public float VerticalVelocity() => rb.velocity.y;
        //
        // public bool IsGrounded() => grounded;
        // public bool IsBraking() => braking;
        // public bool IsFalling() => !grounded && Mathf.Abs(VerticalVelocity()) > 0;
        // public bool IsJumped() => jumped;
        // public bool InHoming() => inHoming;
        // public bool IsRolling() => isRolling;
        // public bool IsWaitingForTheDropDash() => waitingForTheDropDash;
        // public string CurrentSurface()
        // {
        //     if (currentSurface != null) return currentSurface.type.ToString();
        //     return "Concrete";
        // }
        // public bool InLightSpeedDash() => inLightSpeedDash;
        //
        // private bool HomingTargetInSight(Transform target)
        // {
        //     Vector3 viewportPoint = player.Camera.Cam.WorldToViewportPoint(target.position);
        //     return viewportPoint.x is >= 0 and <= 1 &&
        //            viewportPoint.y is >= 0 and <= 1 &&
        //            viewportPoint.z > 0;
        // }
        //
        // private void OnGUI()
        // {
        //     if (!showDebugInfo) return;
        //     
        //     GUIStyle style = new GUIStyle
        //     {
        //         fontSize = 20,
        //         normal =
        //         {
        //             textColor = Color.black
        //         },
        //         font = Resources.Load<Font>("EdgeDisplay-Bold")
        //     };
        //     
        //     GUI.Label(new Rect(10, 10, 100, 20), string.Format("Velocity: {0}", transform.InverseTransformDirection(rb.velocity).ToString("0.0")), style);
        //     GUI.Label(new Rect(10, 55, 100, 20), string.Format("Grounded: {0}", grounded.ToString()), style);
        //     GUI.Label(new Rect(10, 235, 100, 20), string.Format("Jumped: {0}", jumped.ToString()), style);
        //     GUI.Label(new Rect(10, 280, 100, 20), string.Format("Braking: {0}", braking.ToString()), style);
        //     GUI.Label(new Rect(10, 325, 100, 20), string.Format("In Homing: {0}", inHoming.ToString()), style);
        //     GUI.Label(new Rect(10, 370, 100, 20), string.Format("Speed: {0:0.0}", rb.velocity.magnitude), style);
        //     
        //     GUI.Label(new Rect(10, 415, 100, 20), string.Format("Ground Normal: {0}", groundNormal.ToString()), style);
        //     GUI.Label(new Rect(10, 460, 100, 20), string.Format("Surface Type: {0}", CurrentSurface()), style);
        //     GUI.Label(new Rect(10, 505, 100, 20), string.Format("In Loop: {0}", inLoop), style);
        //     GUI.Label(new Rect(10, 550, 100, 20), string.Format("Should Apply Loop Force: {0}", inLoop && rb.velocity.magnitude > inLoopSpeedThreshold), style);
        //     
        //     Vector3 Gravity = new Vector3(0, gravity, 0);
        //     bool Uphill = Vector3.Dot(rb.velocity.normalized, Gravity) < 0;
        //     GUI.Label(new Rect(10, 100, 100, 20), string.Format("Uphill: {0}", Uphill.ToString()), style);
        //     
        //     GUI.Label(new Rect(10, 145, 100, 20), string.Format("Ground Angle: {0:0.0}", groundAngle), style);
        //     GUI.Label(new Rect(10, 190, 100, 20), string.Format("Ground Speed Mod: {0:0.0}", slopeForceOverSpeed.Evaluate(rb.velocity.sqrMagnitude / TopSpeed / TopSpeed)), style);
        // }
        //
        // private void OnCollisionEnter(Collision other)
        // {
        //     if (Vector3.Dot(groundNormal, Vector3.up) > 0.5f)
        //     {
        //         jumped = false;
        //         jumpDashed = false;
        //     }
        //
        //     if (other.transform.TryGetComponent(out HomingTarget target))
        //     {
        //         if (dashCoroutine != null) StopCoroutine(dashCoroutine);
        //         rb.AddForce(Vector3.up * 0.4f, ForceMode.Impulse);
        //         jumpDashed = false;
        //         inHoming = false;
        //         jumpDashCycleTimer = jumpDashTimer;
        //         
        //         OnPlayerHomingEnded?.Invoke();
        //     }
        // }
        //
        // private void OnDrawGizmos()
        // {
        //     if (splineContainer)
        //     {
        //         Gizmos.color = new Color(0.56f, 0.13f, 1f);
        //         Gizmos.DrawWireSphere(splineContainer.transform.TransformPoint(Math.ConvertFloat3ToVector3(splineContainer.Spline.Knots.ToArray()[0].Position)), 0.2f);
        //
        //         Gizmos.color = Color.red;
        //         var length = splineContainer.Spline.GetLength();
        //         lightSpeedDashTimer =
        //             Mathf.Clamp(lightSpeedDashTimer + lightSpeedDashForce * Time.deltaTime, 0, length);
        //         var ourPosWorld = transform.position;
        //         SplineUtility.GetNearestPoint(splineContainer.Spline,
        //             splineContainer.transform.InverseTransformPoint(ourPosWorld), out var nearestPointLocal, out var n, 32, 12);
        //         splineContainer.Spline.Evaluate(n, out var pos, out var dir, out var _);
        //         var nearestPoint = splineContainer.transform.TransformPoint(nearestPointLocal);
        //         Debug.Log(n);
        //         Gizmos.DrawSphere(Math.ConvertFloat3ToVector3(nearestPoint), 0.2f);
        //     }
        // }
    }
}