using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateGoal : StateMove
    {
        private float timer;
        private bool pizdec;
        
        public StateGoal(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            animator.SetFloat("GroundVelocity", 0f);
            animator.SetBool("Goal", true);
            animator.Play("ResultLook", 0);
            rb.velocity = Vector3.zero;
            
            player.HUD.parent.gameObject.SetActive(false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            float d = 3.25f;

            if (timer <= d)
            {
                timer += Time.deltaTime;
            }

            if (timer >= d && !pizdec)
            {
                var rank = player.stage.GetRank();
                animator.Play($"Result {rank}");

                pizdec = true;
            }
        }

        protected override void Move()
        {
            
        }

        public void Set(Transform ring, Transform target)
        {
            rb.position = ring.TransformDirection(target.position);
            rb.rotation = target.rotation;
        }
    }
}