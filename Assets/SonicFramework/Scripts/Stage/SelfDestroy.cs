using Lean.Pool;
using UnityEngine;

namespace SonicFramework
{
    public class SelfDestroy : MonoBehaviour
    {
        [SerializeField] private bool poolObject = true;
        [SerializeField, Min(0)] private float delay;

        private void Start()
        {
            if (poolObject) LeanPool.Despawn(gameObject, delay);
            else Destroy(gameObject, delay);
        }
    }
}