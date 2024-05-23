using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.CameraStates
{
    public class PhotoCameraState : CameraBaseState
    {
        public float x;
        public float y;

        private Vector3 rotationVector;
        private float additiveSpeed;

        private PhotoInputService input;
        
        public PhotoCameraState(FSM fsm, GameObject gameObject, SonicCameraConfig config, Settings settings, PhotoInputService input) : base(fsm, gameObject, config, settings)
        {
            this.input = input;
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.enabled = false;

            additiveSpeed = 5f;
            Time.timeScale = 0;
        }

        public override void Exit()
        {
            base.Exit();
            
            player.Input.enabled = true;

            Time.timeScale = 1;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                fsm.SetState<DefaultCameraState>();
            }

            if (input.WasPressed(InputButton.Screenshot))
            {
                Framework.MakeScreenshot();
            }
            
            x += input.lookInput.x;
            y -= input.lookInput.y;
            y = Mathf.Clamp(y, -90, 90);
            
            rotationVector = new Vector3(y, x, 0);
            
            Vector3 dir = camTransform.forward * input.flyInput.y + camTransform.right * input.flyInput.x;
            
            camTransform.localPosition += dir * (additiveSpeed * Time.unscaledDeltaTime);

            if (input.IsPressed(InputButton.FlyUp))
            {
                camTransform.localPosition += Vector3.up * (additiveSpeed * Time.unscaledDeltaTime);
            }
            else if (input.IsPressed(InputButton.FlyDown))
            {
                camTransform.localPosition += Vector3.down * (additiveSpeed * Time.unscaledDeltaTime);
            }
            
            camTransform.rotation = Quaternion.Euler(rotationVector);
        }
        
        public void Set(float x, float y) { this.x = x; this.y = y; }
    }
}