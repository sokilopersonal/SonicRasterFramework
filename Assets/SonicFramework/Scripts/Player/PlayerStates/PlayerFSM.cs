using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class PlayerFSM : FSM
    {
        public Rigidbody rb;

        public Vector3 Velocity
        {
            get => rb.velocity;
        }

        public Vector3 LocalVelocity
        {
            get => rb.transform.InverseTransformDirection(rb.velocity);
        }

        public void Init(Rigidbody rb)
        {
            this.rb = rb;
        }
    }
}