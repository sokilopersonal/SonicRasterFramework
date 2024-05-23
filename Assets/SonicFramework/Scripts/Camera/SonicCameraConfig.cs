using UnityEngine;

namespace SonicFramework
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Framework/Config", order = 0)]
    public class SonicCameraConfig : ScriptableObject
    {
        [Header("Input")]
        public float sens = 1;
        
        [Header("Dynamic Camera")] 
        public float translationSpeed = 5f;
        public float minDistance = 2;
        public float maxDistance = 5;
        public float minFov = 45;
        public float maxFov = 90;
        public float smoothTime;
        
        [Header("Lean")]
        public float velocitySpeed = 0.5f;
        public float maxLean;
        public float leanThreshold;
        
        [Header("Settings")]
        public Vector3 lookOffset;
        
        [Header("Alignment")]
        public float transitionSpeed;
        public float minAngle;
        
        [Header("Collision")]
        public float radius;
        public float collisionLerpSpeed;
        public LayerMask mask;
    }
}