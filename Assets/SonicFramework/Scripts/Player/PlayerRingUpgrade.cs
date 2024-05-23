using FMODUnity;
using SonicFramework.Events;
using SonicFramework.PlayerStates;
using UnityEngine;

namespace SonicFramework
{
    public class PlayerRingUpgrade : PlayerComponent
    {
        [Header("Upgrade")]
        public int speedLevel = 1;
        [SerializeField] private int speedLevelMax = 5;
        [SerializeField] private float ringTopSpeedAdd;
        [SerializeField] private float ringMaxSpeedAdd;
        private float upgradedTopSpeed;
        private float upgradedMaxSpeed;

        [Header("Sound")]
        [SerializeField] private EventReference soundReference;

        private void Start()
        {
            upgradedTopSpeed = player.Config.topSpeed;
            upgradedMaxSpeed = player.Config.maxSpeed;
        }

        private void OnEnable()
        {
            GlobalSpawnablesEvents.OnOneHundredRingsCollected += OneHundredRingsCollected;
        }

        private void OnDisable()
        {
            GlobalSpawnablesEvents.OnOneHundredRingsCollected -= OneHundredRingsCollected;
        }

        private void OneHundredRingsCollected()
        {
            if (speedLevel < speedLevelMax)
            {
                upgradedTopSpeed += ringTopSpeedAdd;
                upgradedMaxSpeed += ringMaxSpeedAdd;
                speedLevel++;

                RuntimeManager.PlayOneShot(soundReference);
            }
        }

        private void Update()
        {
            ((StateMove)player.fsm.CurrentState).topSpeed = upgradedTopSpeed;
            ((StateMove)player.fsm.CurrentState).maxSpeed = upgradedMaxSpeed;
        }
    }
}