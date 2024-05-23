using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class AutoCameraState : DefaultCameraState
    {
        private float signedAngle;

        public AutoCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            // var result = Quaternion.LookRotation(holder.InverseTransformDirection(player.transform.forward), Vector3.up).eulerAngles;
            // x = result.y;
            
            Quaternion from = Quaternion.FromToRotation(holder.forward, player.transform.forward);
            Quaternion lerp = Quaternion.Slerp(holder.rotation, from * holder.rotation, 4 * Time.deltaTime);
            holder.rotation = lerp;
        }

        public override void Exit()
        {
            base.Exit();
            
            //var result = Quaternion.LookRotation(player.transform.forward, Vector3.up).eulerAngles;
            //fsm.GetState<DefaultCameraState>().Set(result.y, result.x);
            
            holder.forward = player.transform.forward;
        }

        protected override void InputThings()
        {
            
        }

        protected override void CountIdle()
        {
            if (input.magnitude >= 0.5f)
            {
                fsm.SetState<DefaultCameraState>();
            }
        }
    }
}