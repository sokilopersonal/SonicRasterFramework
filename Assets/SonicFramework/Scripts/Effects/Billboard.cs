using System;
using UnityEngine;

namespace SonicFramework.Effects
{
    public class Billboard : MonoBehaviour
    {
        private Transform cam;

        private void Start()
        {
            cam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.forward = cam.forward;
        }
    }
}