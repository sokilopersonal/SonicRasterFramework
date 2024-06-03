using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateDashRing : StateMove
    {
        public float timer;
        public float speed;
        public Transform ring;
        private LayerMask mask;
        private LayerMask dashPanelMask;
        
        public StateDashRing(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            mask = config.groundMask;
            dashPanelMask = config.dashPanelMask;
        }

        public override void Enter()
        {
            //animator.Play("DashRing");
            //animator.SetBool(PlayerHash.DashRing, true);
        }

        public override void Exit()
        {
            //animator.SetBool(PlayerHash.DashRing, false);
        }

        public override void FrameUpdate()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                rb.velocity = ring.up * speed;
            }
            
            if (timer <= 0 && !PredictGround())
            {
                fsm.SetState<StateAir>();
            }
            
            if (PredictGround())
            {
                fsm.SetState<StateGround>();
            }
        }

        protected override void Move()
        {
        }

        private bool PredictGround()
        {
            return Physics.SphereCast(gameObject.transform.position, 0.2f, gameObject.transform.up,
                out _, 1f, mask, QueryTriggerInteraction.Ignore);
        }

        private bool PredictDashPanel()
        {
            return Physics.Raycast(gameObject.transform.position, rb.velocity.normalized,
                out _, 1f, dashPanelMask, QueryTriggerInteraction.Collide);
        }
    }
}