using UnityEngine;

namespace SonicFramework
{
    public class HomingTarget : PlayerContactable
    {
        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Homing");
        }
    }
}