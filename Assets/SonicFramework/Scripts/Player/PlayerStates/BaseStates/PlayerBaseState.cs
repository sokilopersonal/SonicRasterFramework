using SonicFramework.StateMachine;
using UnityEngine;

namespace SonicFramework.PlayerStates
{
    public class PlayerBaseState : State
    {
        protected readonly GameObject gameObject;
        protected readonly Transform transform;
        protected readonly Animator animator;
        
        protected readonly PlayerEffects effects;
        protected readonly PlayerSounds sounds;
        protected readonly CapsuleCollider col;
        protected readonly PlayerBase player;
        protected readonly DataContainer dataContainer;

        protected readonly Camera cam;
        
        public PlayerBaseState(FSM fsm, GameObject gameObject) : base(fsm)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            player = this.gameObject.GetComponent<PlayerBase>();
            animator = player.Model.GetComponent<Animator>();
            effects = gameObject.GetComponentInChildren<PlayerEffects>();
            sounds = gameObject.GetComponentInChildren<PlayerSounds>();
            col = gameObject.GetComponentInChildren<CapsuleCollider>();
            dataContainer = gameObject.GetComponent<PlayerBase>().stage.dataContainer;
            cam = Camera.main;
        }

        public override void BaseUpdate()
        {
            
        }
    }
}
