using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class CameraBaseState : State
    {
        protected Vector2 input;

        protected float sens;
        protected float minAngle;
        protected float minDistance;
        protected float maxDistance;
        protected float minFov;
        protected float maxFov;
        protected float transitionSpeed;
        protected float velocitySpeed;
        protected float translationSpeed;
        protected Vector3 lookOffset;
        protected LayerMask mask;
        protected float radius;
        protected float collisionLerpSpeed;

        public GameObject volume;
        protected readonly GameObject gameObject;
        protected readonly SonicCameraConfig config;
        protected readonly Settings settings;
        protected readonly Camera cam;
        public readonly Transform camTransform;
        protected readonly PlayerBase player;
        public Transform holder;
        
        public CameraBaseState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm)
        {
            this.gameObject = gameObject;
            this.config = config;
            this.settings = settings;
            cam = Camera.main;
            camTransform = cam.transform;
            holder = camTransform.parent;
            player = gameObject.GetComponent<PlayerBase>();

            sens = config.sens;
            minAngle = config.minAngle;
            minDistance = config.minDistance;
            maxDistance = config.maxDistance;
            minFov = config.minFov;
            maxFov = config.maxFov;
            transitionSpeed = config.transitionSpeed;
            velocitySpeed = config.velocitySpeed;
            translationSpeed = config.translationSpeed;
            lookOffset = config.lookOffset;
            mask = config.mask;
            radius = config.radius;
            collisionLerpSpeed = config.collisionLerpSpeed;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            sens = config.sens;
            minAngle = config.minAngle;
            minDistance = config.minDistance;
            maxDistance = config.maxDistance;
            minFov = config.minFov;
            maxFov = config.maxFov;
            transitionSpeed = config.transitionSpeed;
            velocitySpeed = config.velocitySpeed;
            translationSpeed = config.translationSpeed;
            lookOffset = config.lookOffset;
            radius = config.radius;
            collisionLerpSpeed = config.collisionLerpSpeed;
            
            CheckInput();
        }

        protected virtual void CheckInput()
        {
            input = player.Input.lookInput;
        }

        public override void BaseUpdate()
        {
            
        }
    }
}