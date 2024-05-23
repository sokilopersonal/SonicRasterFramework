using System;
using UnityEngine;

namespace SonicFramework
{
    public class HurtBox : PlayerContactable
    {
        private BoxCollider col;

        private void Awake()
        {
            col ??= GetComponent<BoxCollider>();
        }

        private void OnValidate()
        {
            col = GetComponent<BoxCollider>();
        }

        public override void OnColliderContact()
        {
            base.OnColliderContact();
            
            player.Life.StartDamage();
        }

        private void OnDrawGizmos()
        {
            if (col != null)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = new Color(1f, 0f, 0.03f, 0.35f);
                Gizmos.DrawCube(col.center, col.size);
            }
        }
    }
}