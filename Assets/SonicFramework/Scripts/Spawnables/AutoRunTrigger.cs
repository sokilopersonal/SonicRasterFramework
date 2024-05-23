using System;
using SonicFramework.CharacterFlags;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    public class AutoRunTrigger : PlayerContactable
    {
        [SerializeField, Range(0, 15)] private float time;

        private float timer;
        private bool active;
        
        public override void OnContact()
        {
            base.OnContact();

            // player.fsm.GetState<StateAuto>(out var state);
            // state.timer = time;
            // player.fsm.SetState<StateAuto>();

            timer = time;
            player.Flags.Add(Flag.LockedInput);
            player.Flags.Add(Flag.AutoRun);

            active = true;
        }

        private void Update()
        {
            if (active)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    player.Flags.Remove(Flag.LockedInput);
                    player.Flags.Remove(Flag.DisableDeceleration);
                    player.Flags.Remove(Flag.AutoRun);

                    active = false;
                    timer = 0;
                }
            }
        }
    }
}