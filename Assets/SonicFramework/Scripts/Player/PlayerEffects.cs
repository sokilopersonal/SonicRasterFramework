using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using SonicFramework.PlayerStates;
using SonicFramework.StateMachine;
using UnityEngine;
using UnityEngine.Rendering;

namespace SonicFramework
{
    public class PlayerEffects : PlayerComponent
    {
        [Header("Particles")]
        [SerializeField] private ParticleSystem speedEffect;
        public JumpBallModel jumpBall;

        [Header("Trail")]
        public VolumeTrailRenderer trail;
        
        [Header("Steps")]
        [SerializeField] private GameObject snowStep;
        [SerializeField] private GameObject grassStep;
        [SerializeField] private GameObject metalStep;
        
        [Header("Charge")]
        [SerializeField] private ParticleSystem chargeEffect;
        private ParticleSystem runtimeChargeEffect;

        [Header("Jump Dash")]
        [SerializeField] private SelfDestroy hopEffect;
        
        private Dictionary<string, GameObject> steps = new Dictionary<string, GameObject>();
        private Coroutine invincibilityEffect;

        private void Start()
        {
            steps = new Dictionary<string, GameObject>
            {
                [SurfaceType.Concrete.ToString()] = snowStep,
                [SurfaceType.Grass.ToString()] = grassStep,
                [SurfaceType.Metal.ToString()] = metalStep,
            };
            
            jumpBall.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            player.fsm.OnStateEnter += OnStateEnter;
            player.fsm.OnStateExit += OnStateExit;
            
            player.Life.OnDamaged += InvincibilityEffect;
        }

        private void OnDisable()
        {
            player.fsm.OnStateEnter -= OnStateEnter;
            player.fsm.OnStateExit -= OnStateExit;

            player.Life.OnDamaged -= InvincibilityEffect;
        }

        private void InvincibilityEffect()
        {
            if (invincibilityEffect != null)
            {
                StopCoroutine(invincibilityEffect);
                player.Model.modelRenderer.enabled = true;
                invincibilityEffect = null;
            }
            
            invincibilityEffect = StartCoroutine(InEffect());
        }

        private void OnStateEnter(State obj)
        {
            if (obj is StateSpinDashCharge)
            {
                //runtimeChargeEffect = LeanPool.Spawn(chargeEffect, transform.parent.position - transform.forward * 0.3f, transform.parent.rotation);
            }

            if (obj is StateJumpDash)
            {
                //LeanPool.Spawn(hopEffect, transform.parent.position + transform.parent.forward, Quaternion.LookRotation(-transform.forward, Vector3.up));
            }
            
            if (obj is StateHoming)
            {
                //LeanPool.Spawn(hopEffect, transform.parent.position + transform.parent.forward, Quaternion.LookRotation(-transform.forward, Vector3.up));
            }
        }

        private void OnStateExit(State obj)
        {
            if (obj is StateSpinDashCharge)
            {
                //LeanPool.Despawn(runtimeChargeEffect, 3f);
            }
        }

        private void Update()
        {
            WindEffect();
        }

        private void WindEffect()
        {
            speedEffect.gameObject.SetActive(player.fsm.Velocity.magnitude > 15);
        }

        public void EnableJumpBall(bool value)
        {
            jumpBall.gameObject.SetActive(value);
            player.Model.modelRenderer.enabled = !value;
        }

        public void Animate()
        {
            jumpBall.Animate();
        }

        public void PlayStepEffect()
        {
            if (player.fsm.Velocity.magnitude > 15f)
            {
                player.fsm.GetState(out StateGround state);
                var step = steps[state.SurfaceName()];
                var instanced = LeanPool.Spawn(step, player.Model.foot.position + player.transform.up * 0.2f, transform.parent.rotation);
            
                LeanPool.Despawn(instanced, 2f);
            }
        }

        private IEnumerator InEffect()
        {
            float delay = 0.1f;
            int count = 20;

            var r = player.Model.modelRenderer;

            for (int i = 0; i < count; i++)
            {
                r.enabled = false;
                yield return new WaitForSeconds(delay);
                r.enabled = true;
                yield return new WaitForSeconds(delay);
            }
            
            r.enabled = true;
        }
    }
}