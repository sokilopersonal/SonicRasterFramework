using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateAuto : StateGround
    {
        public float timer;
        
        public StateAuto(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (timer <= 0)
            {
                fsm.SetState<StateGround>();
                Debug.Log("pochemy");
            }
        }

        protected override void CheckInput()
        {
            
        }

        protected override void CheckButtons()
        {
            
        }

        protected override void Decel(float minDecelRate, float maxDecelRate)
        {
            
        }
    }
}