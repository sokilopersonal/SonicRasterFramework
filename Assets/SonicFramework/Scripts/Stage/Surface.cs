using UnityEngine;

namespace SonicFramework
{
    public enum SurfaceType
    {
        Concrete,
        Grass,
        Metal
    }
    
    public class Surface : MonoBehaviour
    {
        public SurfaceType type;
    }
}