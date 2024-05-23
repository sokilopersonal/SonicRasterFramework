using FMODUnity;
using UnityEngine;

namespace SonicFramework
{
    public class Enemy : HomingTarget
    {
        [SerializeField] private EventReference soundReference;
        
        public override void OnContact()
        {
            // player.fsm.SetState<StateAfterHoming>();
            //
            // Destroy(gameObject);
        }

        private void OnDestroy()
        {
            RuntimeManager.PlayOneShot(soundReference, transform.position);
        }
    }
}