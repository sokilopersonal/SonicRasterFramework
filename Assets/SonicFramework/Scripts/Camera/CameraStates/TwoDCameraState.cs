using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class TwoDCameraState : CameraBaseState
    {
        public TwoDCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
        }

        public override void Exit()
        {
            base.Exit();
            
            player.Camera.fsm.GetState<DefaultCameraState>().SetDir(camTransform.forward);
        }

        public override void FrameUpdate()
        {
            Transform obj = volume.transform;
            Vector3 playerLocalPosition = obj.InverseTransformPoint(player.transform.position);
            playerLocalPosition.z = 0;
            Vector3 offset = new Vector3(0, 0.25f, 0f);
            Vector3 target = obj.position + playerLocalPosition + offset;
            holder.rotation = Quaternion.Lerp(holder.rotation, obj.rotation, 24 * Time.deltaTime);
            //holder.position = Vector3.Lerp(holder.position, obj.position + playerLocalPosition + offset, 24 * Time.deltaTime);
            
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, holder.InverseTransformPoint(target), 14 * Time.deltaTime);

            Quaternion lookTarget = Quaternion.Euler(0, 180, 0);
            camTransform.localRotation = Quaternion.Lerp(camTransform.localRotation, lookTarget, 8 * Time.deltaTime);
        }
    }
}