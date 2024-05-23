
using SonicFramework.CharacterFlags;
using UnityEngine;
using Zenject;

namespace SonicFramework
{
    public class PlayerInputService : InputService
    {
        private PlayerBase player;
        
        public Vector2 movementInput;
        public Vector2 lookInput;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            movementInput = Vector2.zero;
            lookInput = Vector2.zero;
        }

        protected override void Update()
        {
            base.Update();
            
            player ??= FindFirstObjectByType<PlayerBase>();

            if (player != null)
            {
                if (!player.Flags.Check(Flag.LockedInput))
                {
                    movementInput = gameInput.Gameplay.Move.ReadValue<Vector2>();
                }
                else
                {
                    movementInput = Vector2.zero;
                }
                
                lookInput = gameInput.Gameplay.Camera.ReadValue<Vector2>();
            }
        }

        public override bool WasPressed(InputButton button)
        {
            return base.WasPressed(button) && !player.Flags.Check(Flag.LockedInput);
        }

        public override bool WasReleased(InputButton button)
        {
            return base.WasReleased(button) && !player.Flags.Check(Flag.LockedInput);
        }

        public override bool IsPressed(InputButton button)
        {
            return base.IsPressed(button) && !player.Flags.Check(Flag.LockedInput);
        }
    }
}