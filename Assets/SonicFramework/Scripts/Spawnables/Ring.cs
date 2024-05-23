using FMODUnity;
using Lean.Pool;
using SonicFramework.Events;
using UnityEngine;
using Zenject;
using UnityEngine.Splines;

namespace SonicFramework
{
    public class Ring : PlayerContactable
    {
        [Inject] private RingManager ringManager;
        [Inject] private Settings settings;

        [SerializeField] private int ringAdd = 1;
        [SerializeField] private float orbitSpeed;
        [SerializeField] private GameObject ringParticlePrefab;
        [SerializeField] private EventReference soundReference;
        
        private bool collected;
        private SphereCollider col;
        private float orbitTimer;
        
        private void Start()
        {
            col = GetComponent<SphereCollider>();
            
            ringManager.AddRing(transform);
        }

        public override void OnContact()
        {
            if (settings.ringPickupType == RingPickupType.Frontiers)
            {
                if (collected)
                {
                    var p = LeanPool.Spawn(ringParticlePrefab, transform.position, Quaternion.identity);
            
                    LeanPool.Despawn(p, 1.25f);
                    
                    gameObject.SetActive(false);
                }
                else
                {
                    var animate = gameObject.AddComponent<SplineAnimate>();
                    animate.Container = player.Model.spline;
                    animate.StartOffset = 0.03f;
                    animate.Alignment = SplineAnimate.AlignmentMode.None;
                    animate.AnimationMethod = SplineAnimate.Method.Speed;
                    animate.MaxSpeed = orbitSpeed;
                    
                    RuntimeManager.PlayOneShot(soundReference, transform.position);
                    AddCount();
                    
                    col.radius = 0.1f;
                }

                collected = true;
            }
            else
            {
                RuntimeManager.PlayOneShot(soundReference, transform.position);
                AddCount();
                
                var p = LeanPool.Spawn(ringParticlePrefab, transform.position, Quaternion.identity);
            
                LeanPool.Despawn(p, 1.25f);
                
                gameObject.SetActive(false);
            }
        }

        private void AddCount()
        {
            for (int i = 0; i < ringAdd; i++)
            {
                stage.dataContainer.RingCount += 1;
            }
            
            stage.dataContainer.Score += ScoreValues.Ring * ringAdd;
            OnRingCollectedInvoke();
        }

        protected virtual void OnRingCollectedInvoke()
        {
            GlobalSpawnablesEvents.OnRingWorldCollected?.Invoke(gameObject);
        }
    }
}