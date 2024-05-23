using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateDropDashForce : StateMove
    {
        public Vector3 dropDashStart;
        private float dropDashPower;
        private float maxDropDashPower;
        
        public StateDropDashForce(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            dropDashPower = config.dropDashPower;
            maxDropDashPower = config.maxDropDashPower;
        }

        public override void Enter()
        {
            base.Enter();

            rb.velocity = Vector3.zero;

            CheckGround(-_groundCheck.up, out var hit);
            
            Vector3 diff = gameObject.transform.position - dropDashStart;
            diff.x = 0;
            diff.z = 0;
                
            float p = Mathf.Clamp(Mathf.Max(dropDashPower, diff.magnitude * 0.025f), dropDashPower, maxDropDashPower);
            //Vector3 dir = gameObject.transform.forward;
            Vector3 dir = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            rb.AddForce(dir * p, ForceMode.Impulse);
            sounds.PlaySound("DropDash");
                
            fsm.SetState<StateRoll>();
        }

        protected override void Move()
        {
            
        }
    }
}