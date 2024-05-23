using System;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using SonicFramework.CameraStates;
using SonicFramework.Events;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    public class GoalRing : PlayerContactable
    {
        [Header("Warning: You can't rotate the goal ring")]
        [Header("because the camera positions will be broken.")]
        [Header("To rotate positions - rotate the 'Targets' object")]
        [Space]
        [Space]
        public Transform sonicTarget;
        public Transform cameraTarget;
        public Transform lookTarget;

        [SerializeField] private EventReference loopReference;
        [SerializeField] private EventReference hitReference;

        private EventInstance loopInstance;

        private float timer;
        private bool hitted;

        private void Awake()
        {
            loopInstance = RuntimeManager.CreateInstance(loopReference);
            loopInstance.set3DAttributes(transform.position.To3DAttributes());
            loopInstance.start();
        }

        public override void OnContact()
        {
            base.OnContact();

            player.Camera.fsm.SetState<GoalCameraState>();
            player.Input.enabled = false;

            hitted = true;
            
            var data = player.stage.dataContainer;
            data.EndScore = data.Score + player.stage.GetTimeBonus();
            data.EndScore += stage.GetRingBonus();
            
            RuntimeManager.PlayOneShot(hitReference);
            loopInstance.stop();
            
            GlobalSpawnablesEvents.OnGoal?.Invoke();
        }

        private void Update()
        {
            transform.eulerAngles = Vector3.zero;
            
            if (hitted)
            {
                timer += Time.deltaTime;

                var v = player.fsm.rb.velocity;
                player.fsm.rb.velocity = Vector3.MoveTowards(v, Vector3.zero, 14 * Time.deltaTime);

                if (timer >= 2)
                {
                    if (player.fsm.CurrentState is not StateGoal)
                    {
                        player.fsm.SetState<StateGoal>();
                    
                        player.Camera.fsm.GetState<GoalCameraState>().Set(transform, cameraTarget, lookTarget);
                        player.fsm.GetState<StateGoal>().Set(transform, sonicTarget);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            float radius = 0.35f;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sonicTarget.position, radius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cameraTarget.position, radius);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(cameraTarget.position, lookTarget.position);

            Gizmos.color = new Color(1f, 0.44f, 0.15f);
            Gizmos.DrawWireSphere(lookTarget.position, radius);
        }

        private void OnDestroy()
        {
            loopInstance.stop();
        }

        private void OnValidate()
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}