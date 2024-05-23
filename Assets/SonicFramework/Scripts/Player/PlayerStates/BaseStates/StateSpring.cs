using SonicFramework.StateMachine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace SonicFramework.PlayerStates
{
    public class SpringData
    {
        public float speed;
        public float time;
        public Transform spring;

        public SpringData(float speed, float time, Transform spring)
        {
            this.speed = speed;
            this.time = time;
            this.spring = spring;
        }
    }

    public class StateSpring : StateMove
    {
        public SpringData data;
        private float timer;
        private float delayTimer;

        public StateSpring(FSM fsm, GameObject gameObject, PlayerMovementConfig config) : base(fsm, gameObject, config)
        {
        }

        public override void Enter()
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("Spring", true);

            timer = data.time;
            delayTimer = 0.5f;

            if (data.time == 0)
            {
                // float distance = Vector3.Distance(data.trajectory[0], 
                //     data.trajectory[^1]);
                // float angle = Vector3.Angle(data.spring.up, Vector3.up);
                // float sqrt = Mathf.Sqrt(distance * 55f);
                // float sin = Mathf.Sin(angle * 2);
                // float v0 = sqrt / sin;
                // float mod = 0.01f;
                // rb.AddForce(data.spring.up * v0 * mod, ForceMode.Impulse);
                rb.velocity = data.spring.up * data.speed;
            }
        }

        public override void Exit()
        {
            base.Exit();

            animator.SetBool("Spring", false);
        }

        public override void FrameUpdate()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            
                Move();
            }
            
            if (delayTimer > 0)
            {
                delayTimer -= Time.deltaTime;
            }
            
            if (timer <= 0 && delayTimer <= 0)
            {
                fsm.SetState<StateAir>();
            }
        }

        public override void PhysicsUpdate()
        {
            Rotate();

            if (PredictGround())
            {
                transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                fsm.SetState<StateGround>();
            }
        }

        protected override void Move()
        {
            rb.velocity = data.spring.up * data.speed;
        }

        protected override void Rotate()
        {
            // transformNormal = Vector3.Slerp(transformNormal, data.spring.up, Time.fixedDeltaTime * 15f);
            //
            // if (!Math.IsApproximate(rb.velocity, Vector3.zero, 0.5f))
            // {
            //     Vector3 lookDir = transform.forward;
            //     lookDir = Vector3.Slerp(transform.forward, data.spring.up, Time.deltaTime * 25f);
            //     Quaternion rotation = Quaternion.LookRotation(lookDir, data.spring.up);
            //     transform.rotation = rotation;
            // }
        }

        private bool PredictGround()
        {
            return Physics.Raycast(_groundCheck.position, transform.up, 1.1f, _mask,
                QueryTriggerInteraction.Ignore);
        }
    }   
}