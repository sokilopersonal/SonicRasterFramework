using UnityEngine;

namespace SonicFramework
{
    public class PhotoInputService : InputService
    {
        public Vector2 flyInput;
        public Vector2 lookInput;

        protected override void OnDisable()
        {
            base.OnDisable();
            
            flyInput = Vector2.zero;
            lookInput = Vector2.zero;
        }

        protected override void Update()
        {
            base.Update();

            flyInput = gameInput.PhotoMode.Fly.ReadValue<Vector2>();
            lookInput = gameInput.PhotoMode.Look.ReadValue<Vector2>();
        }
    }
}