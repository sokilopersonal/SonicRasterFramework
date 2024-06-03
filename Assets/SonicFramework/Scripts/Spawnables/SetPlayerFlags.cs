using System;
using System.Collections.Generic;
using SonicFramework.CharacterFlags;
using UnityEngine;

namespace SonicFramework
{
    public class SetPlayerFlags : PlayerContactable
    {
        [SerializeField] private List<Flag> flags;
        [SerializeField, Range(1f, 10f)] private float time;

        private float timer;
        
        public static Action<int> OnFlagSet;

        private void OnEnable()
        {
            OnFlagSet += OnFlagSetted;
        }

        private void OnDisable()
        {
            OnFlagSet -= OnFlagSetted;
        }

        public override void OnContact()
        {
            base.OnContact();

            foreach (Flag flag in flags)
            {
                player.Flags.Add(flag);
            }
            
            OnFlagSet?.Invoke(gameObject.GetInstanceID());
            timer = time;
        }

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                OnTimerEnd();
            }
        }

        private void OnTimerEnd()
        {
            foreach (Flag flag in flags)
            {
                player.Flags.Remove(flag);
            }

            timer = 0;
        }

        private void OnFlagSetted(int id)
        {
            if (id != gameObject.GetInstanceID())
            {
                timer = 0;
            }
        }
    }
}