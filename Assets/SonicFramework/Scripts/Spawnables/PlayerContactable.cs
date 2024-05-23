using System;
using UnityEngine;
using Zenject;

namespace SonicFramework
{
    public class PlayerContactable : MonoBehaviour, IPlayerContactable
    {
        [Inject] public PlayerBase player { get; set; }
        [Inject] protected Stage stage { get; set; }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.CompareTag("Player"))
                {
                    OnContact();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (player)
            {
                OnDiscontact();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Player"))
            {
                OnColliderContact();
            }
        }

        public virtual void OnContact()
        {
            
        }

        public virtual void OnDiscontact()
        {
            
        }

        public virtual void OnColliderContact()
        {
            
        }
    }
}