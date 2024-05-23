using FMODUnity;
using SonicFramework.CameraStates;
using SonicFramework.CharacterFlags;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    [SelectionBase]
    public class DashPanel : PlayerContactable
    {
        [Header("Parameters")]
        [SerializeField, Range(0, 10)] private float time;
        [SerializeField, Range(0, 200)] private int speed;
        [SerializeField] private bool transformCamera = true;

        [Header("Sound")] 
        [SerializeField] private EventReference soundReference;

        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("DashPanel");
        }

        public override void OnContact()
        {
            player.fsm.rb.velocity = Vector3.zero;
            player.Flags.Remove(Flag.LockedInput);
                
            RuntimeManager.PlayOneShot(soundReference, transform.position);
            if (transformCamera) player.Camera.fsm.GetState<DefaultCameraState>().SetDir(transform.forward);
            
            player.transform.position = transform.position + transform.up * 0.1f;
            player.transform.up = transform.up;
            
            if (speed != 0) player.fsm.rb.velocity = transform.forward * speed;
            if (time != 0)
            {
                if (player.fsm.GetState<StateAuto>(out var state))
                {
                    state.timer = time;
                }
                
                player.fsm.SetState<StateAuto>();
            }
            else
            {
                player.fsm.SetState<StateGround>();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position + transform.forward, transform.forward * 5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * speed * time);
        }
    }
}