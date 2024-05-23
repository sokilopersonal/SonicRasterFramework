using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class DelayedCameraState : DefaultCameraState
    {
        private float timer;
        private float delay;

        public DelayedCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
            delay = 0.125f;
        }

        public override void Enter()
        {
            timer = delay;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                fsm.SetState<RecoveryCameraState>();
            }
        }

        protected override void CheckInput()
        {
        }

        protected override void InputThings()
        {
        }

        protected override void Positioning()
        {
        }

        protected override void Collision()
        {
        }

        protected override void Rotation()
        {
        }
    }
}