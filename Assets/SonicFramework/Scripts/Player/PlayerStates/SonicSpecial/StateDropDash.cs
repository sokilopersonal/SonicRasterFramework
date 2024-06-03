using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateDropDash : StateAir
    {
        private Vector3 dropDashStart;
        
        public StateDropDash(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            
            dropDashStart = gameObject.transform.position;

            if (fsm.GetState<StateDropDashForce>(out var state))
            {
                state.dropDashStart = dropDashStart;
            }
            
            sounds.PlaySound("ChargeDropDash");
            //animator.SetBool(PlayerHash.Rolling, true);

            effects.EnableJumpBall(true);
            effects.Animate();
        }

        public override void Exit()
        {
            base.Exit();
            
            //animator.SetBool(PlayerHash.Rolling, false);
            effects.EnableJumpBall(false);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (player.Input.WasReleased(InputButton.Roll))
            {
                fsm.SetState<StateAir>();
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            if (CheckGround(-_groundCheck.up, out _))
            {
                fsm.SetState<StateDropDashForce>();
            }
        }
    }
}