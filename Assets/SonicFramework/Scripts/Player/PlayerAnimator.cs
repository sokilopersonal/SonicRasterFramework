using UnityEngine;

namespace SonicFramework
{
    public class PlayerAnimator : PlayerComponent
    {
        public Animator Animator { get; private set; }

        public override void Init(PlayerBase player)
        {
            base.Init(player);

            Animator = GetComponent<Animator>();
        }
    }
}