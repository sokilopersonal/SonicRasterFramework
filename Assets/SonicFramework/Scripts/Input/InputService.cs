using System;
using UnityEngine;

namespace SonicFramework
{
    public class InputService : MonoBehaviour, IInputService
    {
        public GameInput gameInput { get; set; }

        protected virtual void Awake()
        {
            gameInput = new GameInput();
        }

        protected virtual void OnEnable()
        {
            gameInput.Enable();
        }

        protected virtual void OnDisable()
        {
            gameInput.Disable();
        }

        protected virtual void Update()
        {
            
        }

        public virtual bool WasPressed(InputButton button)
        {
            return gameInput.FindAction(button.ToString()).WasPressedThisFrame();
        }

        public virtual bool WasReleased(InputButton button)
        {
            return gameInput.FindAction(button.ToString()).WasReleasedThisFrame();
        }

        public virtual bool IsPressed(InputButton button)
        {
            return gameInput.FindAction(button.ToString()).IsPressed();
        }

        public bool IsAnyButtonPressed()
        {
            return gameInput.Gameplay.Move.IsPressed() 
                   || gameInput.Gameplay.Camera.IsPressed()
                   || gameInput.Gameplay.Jump.IsPressed()
                   || gameInput.Gameplay.Roll.IsPressed()
                   || gameInput.Gameplay.Charge.IsPressed();
        }
    }
}