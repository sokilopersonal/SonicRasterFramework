using System;
using FMODUnity;
using SonicFramework.Entity;
using SonicFramework.PlayerStates;
using UnityEngine;
using Zenject;

namespace SonicFramework
{
    public class PlayerLife : PlayerComponent, IDamageable
    {
        public Action OnDamaged;
        
        [SerializeField] private PhysicsRing ringPrefab;
        [SerializeField] private int maxRings = 120;
        [SerializeField] private float invincibilityTime = 2f;

        [SerializeField] private EventReference soundReference;

        [Inject] private IInstantiator instantiator;
        private float timer;

        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }

        private void OnEnable()
        {
            OnDamaged += OnPlayerDamaged;
        }

        private void OnDisable()
        {
            OnDamaged -= OnPlayerDamaged;
        }

        private void OnPlayerDamaged()
        {
            player.fsm.SetState<StateDamage>();
            
            var data = player.stage.dataContainer;

            if (data.RingCount > 0)
            {
                int losedRings = Mathf.Min(maxRings, data.RingCount);

                for (int i = 0; i < losedRings; i++)
                {
                    var ring = instantiator.InstantiatePrefabForComponent<PhysicsRing>(ringPrefab, transform.position,
                        Quaternion.identity, null);
                    Vector3 dir = Quaternion.Euler(0, 90, 0) * (Quaternion.AngleAxis(360 / losedRings * i, Vector3.up) * transform.forward);
                    ring.ApplyForce(dir * 450f);
                    ring.ApplyForce(Vector3.up * 400f);
                }
            }
            
            RuntimeManager.PlayOneShot(soundReference);
            data.RingCount = 0;
            timer = invincibilityTime;
        }

        public void StartDamage()
        {
            if (timer > 0) return;
            
            OnDamaged?.Invoke();
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}