using UnityEngine;

namespace SonicFramework.Materials
{
    public class Rainbow : MonoBehaviour
    {
        private Renderer renderer;

        [SerializeField] private float offset;

        private void OnValidate()
        {
            renderer ??= GetComponent<Renderer>();
            
            var block = new MaterialPropertyBlock();
            block.SetFloat("_Offset", offset);
            
            renderer.SetPropertyBlock(block);
        }
    }
}