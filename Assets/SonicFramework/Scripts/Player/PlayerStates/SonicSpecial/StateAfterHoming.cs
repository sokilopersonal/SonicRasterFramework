using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateAfterHoming : StateAir
    {
        private float timer;
        private Vector3 lastVelocity;
        private Vector3 lastTargetPosition;
        
        public StateAfterHoming(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            
        }

        public override void Enter()
        {
            lastVelocity = rb.velocity;
            
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * (config.homingSpeed * config.homingVelocityKeep), ForceMode.VelocityChange);
            rb.ResetVerticalVelocity();
            rb.AddForce(Vector3.up * config.homingKnockback, ForceMode.Impulse);
            timer = 0.5f;
            
            animator.Play($"After Homing {Random.Range(1, 5)}");
        }

        public override void Exit()
        {
            animator.SetInteger("HomingIndex", -1);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                fsm.SetState<StateAir>();
            }
        }

        public void SetLastTarget(Vector3 target)
        {
            lastTargetPosition = target;
        }
    }
}