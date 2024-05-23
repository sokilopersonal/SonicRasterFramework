using System;
using UnityEngine;

namespace SonicFramework
{
    public class RingDropVisual : MonoBehaviour
    {
        [SerializeField] private int ringCount;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < ringCount; i++)
            {
                Vector3 dir = Quaternion.AngleAxis(360 / ringCount * i, Vector3.up) * Vector3.forward;
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, dir);
            }
        }
    }
}