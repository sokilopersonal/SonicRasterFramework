using UnityEngine;

namespace SonicFramework
{
    public class RingGroup : MonoBehaviour
    {
        private void Start()
        {
            LayerMask mask = LayerMask.NameToLayer("LightDash");
            foreach (Transform child in transform)
            {
                child.gameObject.layer = mask;
                gameObject.layer = mask;
            }
        }
    }
}