using SonicFramework.CameraStates;
using SonicFramework.StateMachine;
using UnityEngine;
using Zenject;

namespace SonicFramework
{
    public class PlayerCamera : PlayerComponent
    {
        [Inject] private PhotoInputService input;
        
        public CameraFSM fsm;

        [SerializeField] private SonicCameraConfig config;
        [SerializeField] private Settings settings;

        private void Awake()
        {
            fsm = new CameraFSM();
            fsm.AddState(new DefaultCameraState(fsm, gameObject, config, settings));
            fsm.AddState(new AutoCameraState(fsm, gameObject, config, settings));
            fsm.AddState(new PhotoCameraState(fsm, gameObject, config, settings, input));
            fsm.AddState(new DelayedCameraState(fsm, gameObject, config, settings));
            fsm.AddState(new RecoveryCameraState(fsm, gameObject, config, settings));
            
            // Pans
            fsm.AddState(new PanLookAtState(fsm, gameObject, config, settings));
            fsm.AddState(new PanLookAtFromForwardState(fsm, gameObject, config, settings));
            fsm.AddState(new TwoDCameraState(fsm, gameObject, config, settings));
            fsm.AddState(new GoalCameraState(fsm, gameObject, config, settings));
            
            fsm.SetState<DefaultCameraState>();
        }

        private void Update()
        {
            fsm?.FrameUpdate();
        }

        private void FixedUpdate()
        {
            fsm?.PhysicsUpdate();
        }

        //         private void Awake()
//         {
//             Cam = Camera.main;
//             cameraTransform = Cam.transform;
//             holder = cameraTransform.parent;
//             
//             rotationVector = Quaternion.LookRotation(transform.forward).eulerAngles;
//             x = rotationVector.y;
//             y = rotationVector.x;
//             
//             cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.LookRotation(
//                 transform.position - cameraTransform.position + lookOffset,
//                 _normal), translationSpeed * Time.deltaTime);
//             
//             dynamicDistance = 3;
//             distance = dynamicDistance;
//             dynamicFov = Cam.fieldOfView;
//         }
//
//         private void Update()
//         {
//             if (!_startLocked)
//             {
//                 if (mode == CameraMode.Sonic)
//                 {
//                     SonicCamera();
//                 }
//                 else
//                 {
//                     PhotoModeCamera();
//                 }
//
//                 if (Input.GetKeyDown(KeyCode.Tab))
//                 {
//                     mode = mode == CameraMode.Sonic ? CameraMode.Photo : CameraMode.Sonic;
//
//                     if (mode == CameraMode.Photo)
//                     {
//                         holder.up = Vector3.up;
//                     }
//                 
//                     photoX = x;
//                     photoY = y;
//                     photoZ = 0;
//                 
//                     Time.timeScale = mode == CameraMode.Sonic ? 1 : 0;
//
//                     photoAdditiveSpeed = 2;
//                 }
//             }
//             else
//             {
//                 cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(transform.position - cameraTransform.position), 9 * Time.deltaTime);
//             }
//         }
//
//         private void SonicCamera()
//         {
//             x += Input.GetAxis("Mouse X") * sens;
//             y -= Input.GetAxis("Mouse Y") * sens;
//             y = Mathf.Clamp(y, -60, 60);
//             
//             rotationVector = new Vector3(y, x, 0);
//             
//             if (player.fsm.CurrentState is StateGround state)
//             {
//                 // var dot = Vector3.Dot(state.groundNormal, Vector3.up);
//                 // if (state.groundAngle > minAngle && dot > minDot)
//                 // {
//                 //     _normal = state.groundNormal;
//                 // }
//                 // else
//                 // {
//                 //     _normal = Vector3.up;
//                 // }
//
//                 if (state.groundAngle > minAngle)
//                 {
//                     _normal = state.groundNormal;
//                 }
//                 else
//                 {
//                     _normal = Vector3.up;
//                 }
//             }
//             else
//             {
//                 _normal = Vector3.up;
//             }
//
//             dynamicDistance = Mathf.Lerp(minDistance, maxDistance,
//                 player.fsm.Velocity.magnitude / player.config.topSpeed / 2.5f);
//             distance = Mathf.Lerp(distance, dynamicDistance, 12 * Time.deltaTime);
//             
//             if (player.fsm.CurrentState is StateGround or StateAir) dynamicFov = Mathf.Lerp(minFov, maxFov, player.fsm.Velocity.magnitude / player.config.topSpeed / 2.5f);
//             else if (player.fsm.CurrentState is StateSpinDashCharge)
//                 dynamicFov = Mathf.Lerp(dynamicFov, 37, 1.2f * Time.deltaTime);
//             Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, dynamicFov, 12 * Time.deltaTime);
//
//             holder.up = Vector3.Slerp(holder.up, _normal, transitionSpeed * Time.deltaTime);
//
//             holder.position = transform.position;
//             cameraTransform.localPosition = Quaternion.Euler(rotationVector) * new Vector3(0, 0, -distance);
//             cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.LookRotation(
//                 transform.position - cameraTransform.position + lookOffset,
//                 _normal), translationSpeed * Time.deltaTime);
//
//             // if (player.fsm.Velocity.magnitude > 5f)
//             // {
//             //     float velocityAngle = Vector3.SignedAngle(Vector3.forward, player.fsm.Velocity, _normal);
//             //     x = Mathf.LerpAngle(x, velocityAngle, velocitySpeed * Time.deltaTime);
//             // }
//
//             Collision();
//         }
//
//         private void PhotoModeCamera()
//         {
//             //player.Physics.MovementLocked = mode != CameraMode.Sonic;
//             
//             dynamicDistance = 2;
//             dynamicFov = 60;
//
//             Vector2 photoInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//             Vector3 dir = cameraTransform.forward * photoInput.y + cameraTransform.right * photoInput.x;
//
//             photoAdditiveSpeed += Input.mouseScrollDelta.y;
//             photoAdditiveSpeed = Mathf.Clamp(photoAdditiveSpeed, 0, 30);
//             
//             if (Input.GetMouseButton(1))
//             {
//                 photoZ += Input.GetAxisRaw("Mouse X") * sens;
//                 Cam.fieldOfView -= Input.GetAxisRaw("Mouse Y") * sens;
//                 Cam.fieldOfView = Mathf.Clamp(Cam.fieldOfView, 2, 150);
//             }
//             else
//             {
//                 photoX += Input.GetAxisRaw("Mouse X") * sens;
//                 photoY -= Input.GetAxisRaw("Mouse Y") * sens;
//             }
//
//             if (Input.GetKeyDown(KeyCode.R))
//             {
//                 photoZ = 0;
//                 Cam.fieldOfView = 60;
//             }
//
//             if (Input.GetKeyDown(KeyCode.LeftShift))
//             {
//                 StartCoroutine(StepOneFrame());
//             }
//             
//             photoY = Mathf.Clamp(photoY, -89, 89);
//             cameraTransform.localPosition += dir * ((0.5f + photoAdditiveSpeed) * Time.unscaledDeltaTime);
//
//             if (Input.GetKey(KeyCode.Space))
//             {
//                 cameraTransform.localPosition += Vector3.up * ((0.5f + photoAdditiveSpeed) * Time.unscaledDeltaTime);
//             }
//             else if (Input.GetKey(KeyCode.LeftControl))
//             {
//                 cameraTransform.localPosition += Vector3.down * ((0.5f + photoAdditiveSpeed) * Time.unscaledDeltaTime);
//             }
//             
//             cameraTransform.rotation = Quaternion.Euler(photoY, photoX, -photoZ);
//
//             if (Input.GetMouseButtonDown(0))
//             {
//                 StartCoroutine(Framework.MakeScreenshot());
//             }
//             
//             // Only for editor
// #if UNITY_EDITOR
//             if (Input.GetMouseButton(3))
//             {
//                 transform.position = cameraTransform.position + cameraTransform.forward * 2;
//             }
// #endif
//             
//             Vector3 clampedPosition = cameraTransform.localPosition;
//             clampedPosition.x = Mathf.Clamp(clampedPosition.x, -photoBoxSize, photoBoxSize);
//             clampedPosition.y = Mathf.Clamp(clampedPosition.y, -photoBoxSize, photoBoxSize);
//             clampedPosition.z = Mathf.Clamp(clampedPosition.z, -photoBoxSize, photoBoxSize);
//             cameraTransform.localPosition = clampedPosition;
//         }
//
//         private void OnDestroy()
//         {
//             if (startCor != null) StopCoroutine(startCor);
//         }
//
//         private IEnumerator StepOneFrame()
//         {
//             Time.timeScale = 1;
//             yield return new WaitForFixedUpdate();
//             Time.timeScale = 0;
//         }
//
//         private void Collision()
//         {
//             float result = Physics.SphereCast(transform.position, radius, -cameraTransform.forward, out RaycastHit hit, distance, mask, QueryTriggerInteraction.Ignore)
//                 ? hit.distance - radius
//                 : distance;
//             collisionResult = Mathf.Lerp(collisionResult, result, collisionLerpSpeed * Time.deltaTime);
//
//             cameraTransform.position = transform.position + transform.TransformDirection(lookOffset) - cameraTransform.forward * collisionResult;
//         }
//
//         public void SetDir(Vector3 dir)
//         {
//             rotationVector = Quaternion.LookRotation(dir).eulerAngles;
//             x = rotationVector.y;
//             
//             cameraTransform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position + lookOffset, _normal);
//         }
//
//         public void Lock(bool value) => _startLocked = value;
//
//         public IEnumerator SetStart(PlayerStartCameraData data)
//         {
//             _startLocked = true;
//
//             Vector3 a = holder.InverseTransformPoint(data.point1.position);
//             Vector3 b = holder.InverseTransformPoint(data.point2.position);
//
//             cameraTransform.localPosition = a;
//             cameraTransform.rotation = data.point1.rotation;
//             cameraTransform.DOLocalMove(b, 2.75f).SetEase((DG.Tweening.Ease)Ease.InOutSine);
//
//             y = data.startY;
//
//             yield return new WaitForSeconds(3.1f);
//             _startLocked = false;
//             
//             Destroy(data.point1.gameObject);
//             Destroy(data.point2.gameObject);
//         }
    }

    public class PlayerStartCameraData
    {
        public Transform point1;
        public Transform point2;
        public float startY;
    }
}