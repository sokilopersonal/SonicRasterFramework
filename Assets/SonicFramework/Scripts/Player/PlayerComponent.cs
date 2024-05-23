using UnityEngine;

namespace SonicFramework
{
    public class PlayerComponent : MonoBehaviour
    {
        public PlayerBase player { get; private set; }

        public virtual void Init(PlayerBase player)
        {
            this.player = player;
        }
    }
}