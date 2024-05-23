using System;
using UnityEngine;

namespace SonicFramework
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float speed;

        private void Update()
        {
            transform.Rotate(Vector3.up * (speed * Time.deltaTime));
        }
    }
}