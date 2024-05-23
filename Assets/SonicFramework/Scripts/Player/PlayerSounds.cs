using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using SonicFramework.PlayerStates;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace SonicFramework
{
    public class PlayerSounds : PlayerComponent
    {
        [Header("Sound References")]
        [SerializeField] private EventReference jumpReference;
        [SerializeField] private EventReference jumpDashReference;
        [SerializeField] private EventReference rollReference;
        [SerializeField] private EventReference chargeDropDashReference;
        [SerializeField] private EventReference dropDashReference;
        [SerializeField] private EventReference stepReference;
        [SerializeField] private EventReference landReference;
        [SerializeField] private EventReference spinDashReference;
        [SerializeField] private EventReference windReference;
        [SerializeField] private EventReference resultReference;

        [Header("Wind Sound")]
        [SerializeField] private float speedThreshold;
        [SerializeField] private float maxSpeedThreshold;
        private float windPower;

        public EventInstance stepsInstance;
        public EventInstance landInstance;
        public EventInstance spinDashInstance;
        public EventInstance rollInstance;
        public EventInstance windInstance;
        public EventInstance resultInstance;
        
        private Dictionary<string, int> steps;
        
        public override void Init(PlayerBase player)
        {
            base.Init(player);
            
            steps = new Dictionary<string, int>
            {
                [SurfaceType.Concrete.ToString()] = 0,
                [SurfaceType.Grass.ToString()] = 1,
                [SurfaceType.Metal.ToString()] = 2
            };
        }

        private void Start()
        {
            stepsInstance = RuntimeManager.CreateInstance(stepReference);
            resultInstance = RuntimeManager.CreateInstance(resultReference);
            //landInstance = RuntimeManager.CreateInstance(landReference);
            spinDashInstance = RuntimeManager.CreateInstance(spinDashReference);
            rollInstance = RuntimeManager.CreateInstance(rollReference);
            //windInstance = RuntimeManager.CreateInstance(windReference);
            //windInstance.start();

            stepsInstance.set3DAttributes(player.Model.foot.position.To3DAttributes());
            landInstance.set3DAttributes(player.Model.foot.position.To3DAttributes());
            spinDashInstance.set3DAttributes(player.Model.foot.position.To3DAttributes());
            rollInstance.set3DAttributes(player.Model.foot.position.To3DAttributes());
        }

        private void OnEnable()
        {
            player.HUD.OnHUDResult += OnHUDResult;
        }

        private void OnDisable()
        {
            player.HUD.OnHUDResult -= OnHUDResult;
        }

        private void OnHUDResult()
        {
            resultInstance.setParameterByNameWithLabel("Rank", player.stage.GetRank().ToString());
            RuntimeManager.AttachInstanceToGameObject(resultInstance, transform, player.fsm.rb);
            
            resultInstance.start();
        }

        private void Update()
        {
            if (stepsInstance.isValid())
            {
                if (player.fsm.CurrentState is StateGround ground)
                {
                    stepsInstance.setParameterByName("SurfaceType", steps[ground.SurfaceName()]);
                    //landInstance.setParameterByName("SurfaceType", steps[ground.SurfaceName()]);
                }
            }

            if (player.fsm.Velocity.magnitude >= speedThreshold)
            {
                windInstance.getParameterByName("Wind_Intensity", out var result);
                float value = Mathf.Clamp01(player.fsm.Velocity.magnitude / maxSpeedThreshold);
                windPower = Mathf.Lerp(windPower, value, 4 * Time.deltaTime);
                windInstance.setParameterByName("Wind_Intensity", windPower);
            }
            else
            {
                windInstance.getParameterByName("Wind_Intensity", out var result);
                windInstance.setParameterByName("Wind_Intensity", 0);
            }
        }

        public void PlaySound(string sound)
        {
            switch (sound)
            {
                case "Jump":
                    RuntimeManager.PlayOneShot(jumpReference);
                    break;
                case "Homing":
                    RuntimeManager.PlayOneShot(jumpDashReference);
                    break;
                case "ChargeDropDash":
                    RuntimeManager.PlayOneShot(chargeDropDashReference);
                    break;
                case "Roll":
                    RuntimeManager.PlayOneShot(rollReference);
                    break;
                case "DropDash":
                    RuntimeManager.PlayOneShot(dropDashReference);
                    break;
                case "Land":
                    RuntimeManager.AttachInstanceToGameObject(landInstance, player.Model.foot, player.fsm.rb);
                    landInstance.start();
                    break;
            }
        }

        public void PlayStep()
        {
            RuntimeManager.AttachInstanceToGameObject(stepsInstance, player.Model.foot, player.fsm.rb);
            stepsInstance.start();
        }

        private void OnDestroy()
        {
            windInstance.stop();
            resultInstance.stop();
        }
    }
}