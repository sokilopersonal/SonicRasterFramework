using UnityEngine;

namespace SonicFramework
{
    public static class PlayerHash
    {
        public static readonly int Grounded = Animator.StringToHash("Grounded");
        public static readonly int GroundVelocity = Animator.StringToHash("GroundVelocity");
        public static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        public static readonly int Falling = Animator.StringToHash("Falling");
        public static readonly int Jumped = Animator.StringToHash("Jumped");
        public static readonly int Rolling = Animator.StringToHash("Rolling");
        public static readonly int DashRing = Animator.StringToHash("DashRing");
    }
}