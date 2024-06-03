using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateHoming : StateAir
    {
        private readonly float _homingSpeed;
        private HomingTarget homingTarget;
        
        public StateHoming(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
            _homingSpeed = config.homingSpeed;
        }

        public override void Enter()
        {
            useGravity = false;
            
            effects.trail.emitTime = 99f;
            effects.trail.emit = true;
            
            effects.EnableJumpBall(true);
            sounds.PlaySound("Homing");
            
            //animator.SetBool(PlayerHash.Jumped, true);
            animator.SetBool("InHoming", true);
        }

        public override void Exit()
        {
            useGravity = true;
            
            effects.trail.emitTime = 2;
            
            effects.EnableJumpBall(false);
            
            //animator.SetBool(PlayerHash.Jumped, false);
            animator.SetBool("InHoming", false);
        }

        protected override void Move()
        {
            Vector3 dir = homingTarget.transform.position - gameObject.transform.position;
            rb.velocity = dir.normalized * _homingSpeed;

            if (DistanceToTarget() < 1.75f)
            {
                if (homingTarget is Enemy)
                {
                    fsm.SetState<StateAfterHoming>();
                    player.stage.dataContainer.Score += ScoreValues.EnemyDestroyed;
                    Object.Destroy(homingTarget.gameObject);
                }
            }
        }

        protected override void CheckInput()
        {
            
        }

        protected override void CheckButtons()
        {
            
        }

        private float DistanceToTarget()
        {
            return Vector3.Distance(homingTarget.transform.position, transform.position);
        }

        public void SetTarget(HomingTarget target) => this.homingTarget = target;
    }
}