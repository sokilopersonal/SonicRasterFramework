using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class PanLookAtFromForwardState : CameraBaseState
    {
        public PanLookAtFromForwardState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings) : base(fsm, gameObject, config, settings)
        {
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            Vector3 target = player.transform.position + player.transform.forward * 1.5f;
            camTransform.localPosition = holder.InverseTransformPoint(target);
            camTransform.rotation = Quaternion.Lerp(camTransform.rotation, 
                Quaternion.LookRotation(player.transform.position - camTransform.position, player.transform.up), 12 * Time.deltaTime);
        }
    }
}