using FMODUnity;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    [SelectionBase, RequireComponent(typeof(HomingTarget))]
    public class Spring : PlayerContactable
    {
        [SerializeField, Min(0)] private float speed;
        [SerializeField] private float gravity;
        [SerializeField] private int resolution;
        [SerializeField, Min(0)] private float time;
        [SerializeField] private float yOffset;
        
        [SerializeField] private EventReference soundReference;
        
        public override void OnContact()
        {
            player.transform.position = transform.position + transform.up * yOffset;
            player.transform.rotation = transform.rotation;
                
            RuntimeManager.PlayOneShot(soundReference);
            
            player.fsm.SetState<StateAir>();

            if (player.fsm.GetState<StateSpring>(out var state))
            {
                state.data = new SpringData(speed, time, transform);
                player.fsm.SetState<StateSpring>();
            }
        }
        
        private void OnDrawGizmos()
        {
            DrawTrajectory();
        }

        private void DrawTrajectory()
        {
            Vector3 EndPos = transform.position + Direction() * speed * time;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, EndPos);
            int Steps = 4 * (int)speed;
            float FixedDelta = 1f / 100f;
            Vector3 PredVelocity = Direction() * speed;
            Gizmos.color = Color.green;
            Vector3 PredPos = EndPos;
            Vector3 LastPredPos = EndPos;
            for (var i = 0; i < Steps; i++)
            {
                PredVelocity.y -= 55f * FixedDelta;
                PredPos += PredVelocity * FixedDelta;
                Gizmos.DrawLine(LastPredPos, PredPos);
                LastPredPos = PredPos;
            }
        }
        
        private Vector3 Direction()
        {
            return transform.up;
        }
        
        private Vector3[] CalculateTrajectory()
        {
            Vector3[] trajectory = new Vector3[resolution];
            Vector3 initialPosition = transform.position;
            Vector3 initialVelocity = transform.up * speed;
            float maxTime = 2f * initialVelocity.magnitude / gravity;

            for (int i = 1; i <= resolution; i++)
            {
                float simulationTime = i * maxTime / resolution;
                Vector3 displacement = initialVelocity * simulationTime +
                                       0.5f * Vector3.down * gravity * simulationTime * simulationTime;
                Vector3 drawPoint = initialPosition + displacement;
                
                trajectory[i - 1] = drawPoint;
            }
            
            return trajectory;
        }
    }
}