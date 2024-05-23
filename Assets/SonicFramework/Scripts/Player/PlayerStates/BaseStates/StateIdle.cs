using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateIdle : PlayerBaseState
    {
        public StateIdle(FSM fsm, GameObject gameObject) : base(fsm, gameObject)
        {
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                fsm.SetState<StateGround>();
            }
        }
    }
}
