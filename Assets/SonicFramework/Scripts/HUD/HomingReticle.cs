using FMODUnity;
using UnityEngine;

namespace SonicFramework.HUD
{
    public class HomingReticle : MonoBehaviour
    {
        [SerializeField] private EventReference soundReference;
        
        private void OnEnable()
        {
            RuntimeManager.PlayOneShot(soundReference);
        }
    }
}