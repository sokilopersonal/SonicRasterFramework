using SonicFramework.CameraStates;
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateStartGoal : StateMove
    {
        private float timer;
        private float duration;
        
        public StateStartGoal(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
        }

        public override void Enter()
        {
            base.Enter();

            timer = 0;

            player.Camera.fsm.SetState<GoalCameraState>();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            timer += Time.deltaTime;
            timer = Mathf.Clamp01(timer);

            // Debug.Log(timer);
            //
            // rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, 16 * Time.deltaTime);
            animator.SetFloat(PlayerHash.GroundVelocity, Mathf.Clamp(rb.velocity.magnitude, 0, 70f));

            if (timer >= 1)
            {
                fsm.SetState<StateGoal>();
            }
        }

        protected override void Move()
        {
            
        }
    }
}