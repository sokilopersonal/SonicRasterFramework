using SonicFramework.PlayerStates;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class GoalCameraState : CameraBaseState
    {
        private Transform ring;
        private Transform target;
        private Transform look;
        
        public GoalCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (ring != null)
            {
                camTransform.localPosition = Vector3.zero;
                camTransform.localRotation = Quaternion.identity;
                holder.position = ring.TransformDirection(target.position);
                holder.LookAt(look);
            }

            if (player.fsm.CurrentState is StateGoal)
            {
                cam.fieldOfView = 50;
            }
        }

        public void Set(Transform ring, Transform target, Transform look)
        {
            this.ring = ring;
            this.target = target;
            this.look = look;
        }
    }
}