using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class PanLookAtState : CameraBaseState
    {
        private LookAtData data;
        private float timer;

        public PanLookAtState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            //camTransform.localPosition = Vector3.zero;
        }

        public override void Exit()
        {
            fsm.GetState<DefaultCameraState>().SetDir(player.transform.forward);
        }

        public override void FrameUpdate()
        {
            data.lookTarget = player.transform;
            
            Vector3 position = data.positionSpeed > 0 ? Vector3.Lerp(holder.position, data.positionTarget.position, data.positionSpeed * Time.deltaTime) : data.positionTarget.position;
            holder.position = position;
            Quaternion look = Quaternion.LookRotation(data.lookTarget.position - camTransform.position, Vector3.up);
            Quaternion rotation = data.lookSpeed > 0 ? Quaternion.Lerp(camTransform.rotation, look, data.lookSpeed * Time.deltaTime) : look;
            camTransform.rotation = rotation;

            if (data.time > 0)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    fsm.GetState<RecoveryCameraState>().SetData(new RecoveryData()
                    {
                        duration = 1f,
                        lookAtTarget = false
                    });
                
                    fsm.SetState<RecoveryCameraState>();
                }
            }
        }

        public void UpdateData(LookAtData data)
        {
            this.data = data;

            timer = data.time;
        }
    }
}