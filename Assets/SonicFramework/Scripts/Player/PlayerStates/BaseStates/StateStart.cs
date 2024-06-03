using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateStart : StateGround
    {
        public float speed;
        public float time;
        public int index;

        private float timer;
        
        public StateStart(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
        }

        public override void Enter()
        {
            timer = 3.1f;
            
            //animator.Play("Start 1", 0, 0);
            animator.SetBool("Started", true);
        }

        public override void Exit()
        {
            animator.SetBool("Started", false);
        }

        public override void FrameUpdate()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                rb.velocity = gameObject.transform.forward * speed;

                if (fsm.GetState<StateAuto>(out var state))
                {
                    state.timer = time;
                }
                
                fsm.SetState<StateAuto>();
            }
        }

        protected override void Move()
        {
            
        }

        protected override void CheckButtons()
        {
            
        }

        protected override void CheckInput()
        {
            
        }
    }
}