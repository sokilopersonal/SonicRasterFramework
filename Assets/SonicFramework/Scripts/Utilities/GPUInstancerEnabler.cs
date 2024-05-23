using UnityEngine;

namespace SonicFramework
{
    public class GPUInstancerEnabler : MonoBehaviour
    {
        private void Awake()
        {
            var block = new MaterialPropertyBlock();
            var r = GetComponent<Renderer>();
            r.SetPropertyBlock(block);
        }
    }
}