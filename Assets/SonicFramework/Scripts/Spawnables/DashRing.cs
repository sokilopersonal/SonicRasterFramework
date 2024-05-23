using FMODUnity;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    public class DashRing : PlayerContactable
    {
        [SerializeField] private EventReference soundReference;
        [SerializeField, Range(0.1f, 5f)] private float time = 2;
        [SerializeField, Range(1, 120)] private float speed = 20;
        [SerializeField] private bool keepSpeed = true;

        public override void OnContact()
        {
            RuntimeManager.PlayOneShot(soundReference, transform.position);
            
            player.fsm.SetState<StateDashRing>();
            
            player.transform.position = transform.position;
            player.transform.up = transform.up;
            
            var state = player.fsm.CurrentState as StateDashRing;
            state.timer = time;
            state.speed = speed;
            state.ring = transform;

            if (!keepSpeed) player.fsm.rb.velocity = Vector3.zero;
            
            if (time == 0)
            {
                player.fsm.rb.AddForce(transform.up * speed / 12, ForceMode.Impulse);
            }

            stage.dataContainer.Score += ScoreValues.DashRingPassed;
        }

        private void OnDrawGizmos()
        {
            Vector3 EndPos = transform.position + transform.up * speed * time;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, EndPos);
            int Steps = 8 * (int)speed;
            float FixedDelta = 1f / 100f;
            Vector3 PredVelocity = transform.up * speed;
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
    }
}