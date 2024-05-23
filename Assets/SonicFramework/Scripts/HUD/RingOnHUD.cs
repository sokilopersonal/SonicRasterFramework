using System.Collections;
using Lean.Pool;
using UnityEngine;

namespace SonicFramework.HUD
{
    public class RingOnHUD : MonoBehaviour, IPoolable
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private AnimationCurve fadeCurve;
        [SerializeField] private CanvasGroup group;
        
        private Vector3 end;
        
        public void Initialize(Vector3 end)
        {
            this.end = end;

            StartCoroutine(Move());
            StartCoroutine(Fade());
        }

        IEnumerator Move()
        {
            float t = 0;

            while (t < 1f)
            {
                transform.position = Vector3.Lerp(transform.position, end, curve.Evaluate(t / 1f));
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            
            LeanPool.Despawn(gameObject);
        }

        IEnumerator Fade()
        {
            float t = 0;

            while (t < 1)
            {
                group.alpha = fadeCurve.Evaluate(t);
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        public void OnSpawn()
        {
            group.alpha = 1;
        }

        public void OnDespawn()
        {
            
        }
    }
}