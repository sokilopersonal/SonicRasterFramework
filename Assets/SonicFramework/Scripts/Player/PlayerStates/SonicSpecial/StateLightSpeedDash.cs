
using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class StateLightSpeedDash : StateMove
    {
        private Transform nextRing;
        private Vector3 targetRingPosition;
        private Collider[] rings;
        private int numRingsDetected;
        private bool isRingsDetected;
        private float ringDistance;
        private float closeRingDistance;
        
        public StateLightSpeedDash(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            animator.SetBool("InLightSpeedDash", true);
            
            effects.EnableJumpBall(false);

            rb.velocity = Vector3.zero;
        }

        public override void Exit()
        {
            base.Exit();
            
            animator.SetBool("InLightSpeedDash", false);
        }

        public override void FrameUpdate()
        {
            Move();
            Rotate();
        }

        protected override void Move()
        {
            rings = new Collider[config.lsdMaxRings];
            numRingsDetected = rb.velocity.magnitude > 0.1f ?
                Physics.OverlapSphereNonAlloc(transform.position + (rb.velocity.normalized * config.lsdFindFirstDistance), config.lsdFindFirstRadius, rings, config.ringMask) :
                Physics.OverlapSphereNonAlloc(transform.position + (transform.forward * config.lsdFindFirstDistance), config.lsdFindFirstRadius, rings, config.ringMask);
            isRingsDetected = numRingsDetected > 0;

            if (isRingsDetected)
            {
                closeRingDistance = Mathf.Infinity;
                for (int i = 0; i < numRingsDetected; i++)
                {
                    ringDistance = Vector3.Distance(transform.position, rings[i].transform.position);
                    if (ringDistance < closeRingDistance)
                    {
                        closeRingDistance = ringDistance;
                        nextRing = rings[i].transform;
                    }
                }
                
                rb.velocity = config.lsdSpeed * (nextRing.position - transform.position).normalized;
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.forward * config.lsdSpeed);
                fsm.SetState<StateAir>();
            }
        }

        protected override void Rotate()
        {
            transformNormal = Vector3.Slerp(transformNormal, Vector3.up, Time.fixedDeltaTime * 12f);

            if (!Math.IsApproximate(rb.velocity, Vector3.zero, 0.5f))
            {
                Vector3 lookDir = gameObject.transform.forward;
                lookDir = Vector3.Slerp(gameObject.transform.forward, rb.velocity.normalized, Time.deltaTime * 12f);
                Quaternion rotation = Quaternion.LookRotation(lookDir, transformNormal);
                gameObject.transform.rotation = rotation;
            }
        }

        // private Transform GetRing()
        // {
        //     
        // }
    }
}