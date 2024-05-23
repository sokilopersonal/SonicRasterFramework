using System;
using NaughtyAttributes;
using SonicFramework.CameraStates;
using UnityEngine;

namespace SonicFramework
{
    public enum CameraVolumeType
    {
        LookAt,
        LookAtFromForward,
        TwoD,
    }
    
    public class CameraVolume : PlayerContactable
    {
        [Header("Settings")] 
        [SerializeField, ShowIf(nameof(type), CameraVolumeType.LookAt)] private LookAtData lookAtData;
        [SerializeField] private CameraVolumeType type;

        public override void OnContact()
        {
            base.OnContact();

            SwitchPan();
        }

        public override void OnDiscontact()
        {
            // player.Camera.fsm.GetState<DefaultCameraState>(out var state);
            // Vector3 dir = Quaternion.LookRotation(player.transform.forward).eulerAngles;
            //
            // state.Set(dir.y, dir.x);
            //
            // player.Camera.fsm.SetState<DefaultCameraState>();

            if (lookAtData.time == 0)
            {
                player.Camera.fsm.SetState<DefaultCameraState>();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.transform.parent.CompareTag("Player")) return;

            //SwitchPan();
        }

        private void SwitchPan()
        {
            switch (type)
            {
                case CameraVolumeType.LookAt:
                    if (player.Camera.fsm.GetState<PanLookAtState>(out var pan))
                    {
                        if (lookAtData == null) lookAtData.lookTarget = player.transform;
                        pan.UpdateData(lookAtData);
                
                        player.Camera.fsm.SetState<PanLookAtState>();
                    }
                    break;
                case CameraVolumeType.LookAtFromForward:
                    if (player.Camera.fsm.GetState<PanLookAtFromForwardState>(out var pan2))
                    {
                        player.Camera.fsm.SetState<PanLookAtFromForwardState>();
                    }
                    break;
                case CameraVolumeType.TwoD:
                    if (player.Camera.fsm.GetState<TwoDCameraState>(out var pan3))
                    {
                        pan3.volume = gameObject;
                        
                        player.Camera.fsm.SetState<TwoDCameraState>();
                    }
                    break;
            }
        }
    }
}
