using UnityEngine;

namespace SonicFramework
{
    public enum RingPickupType
    {
        Frontiers,
        Unleashed
    }
    
    [CreateAssetMenu(fileName = "Settings", menuName = "Framework/Settings", order = 0)]
    public class Settings : ScriptableObject
    {
        [Header("Camera")]
        public bool enableCameraLeaning;
        public bool slopeAlignment = true;
        
        [Header("HUD")]
        public RingPickupType ringPickupType;
    }
}