using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class RecoveryData
    {
        public float timer;
        public float duration;
        public bool lookAtTarget;

        public RecoveryData()
        {
            
        }
        
        public RecoveryData(float duration, bool lookAtTarget = false)
        {
            timer = 0;
            this.duration = duration;
            this.lookAtTarget = lookAtTarget;
        }
    }
    
    public class RecoveryCameraState : DefaultCameraState
    {
        public RecoveryData data;

        public RecoveryCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
        }

        public override void Enter()
        {
            //player.Camera.fsm.GetState<DefaultCameraState>(out var state);
        }

        public override void Exit()
        {
            
        }

        public override void FrameUpdate()
        {
            Execute();
        }

        public void SetData(RecoveryData data)
        {
            this.data = data;
        }

        private void Execute()
        {
            Dynamic();
            Alignment();
            if (data.lookAtTarget) Rotation();
            
            holder.position = Vector3.Lerp(holder.position, player.transform.position, data.timer / data.duration);
            data.timer += Time.deltaTime;

            if (data.timer >= data.duration)
            {
                fsm.SetState<DefaultCameraState>();
            }
        }
    }
}