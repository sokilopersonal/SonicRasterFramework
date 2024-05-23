using System;
using UnityEngine;

namespace SonicFramework
{
    [ExecuteInEditMode]
    public class GroundSetter : MonoBehaviour
    {
        private void Update()
        {
            // Get all the transforms and set the layer
            // var transforms = GetComponentsInChildren<Transform>();
            //
            // foreach (var t in transforms)
            // {
            //     t.gameObject.layer = LayerMask.NameToLayer("Ground");
            // }
        }
    }
}