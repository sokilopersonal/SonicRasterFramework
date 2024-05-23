using UnityEngine;

namespace SonicFramework
{
    public static class RigidbodyExtensions
    {
        public static void ResetVerticalVelocity(this Rigidbody rb)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

        public static void ResetVelocity(this Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
        }
    }
}