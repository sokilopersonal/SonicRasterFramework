using System;
using SonicFramework.PlayerStates;
using UnityEngine;
using Zenject;

namespace SonicFramework
{
    public class PlayerBase : MonoBehaviour
    {
        public PlayerFSM fsm;
        
        [field: SerializeField] public PlayerCamera Camera { get; private set; }
        [field: SerializeField] public PlayerEffects Effects { get; private set; }
        [field: SerializeField] public PlayerSounds Sounds { get; private set; }
        [field: SerializeField] public PlayerModel Model { get; private set; }
        [field: SerializeField] public PlayerRingUpgrade RingUpgrade { get; private set; }
        [field: SerializeField] public PlayerLife Life { get; private set; } 
        [field: SerializeField] public PlayerFlags Flags { get; private set; }
        [Inject] public PlayerHUD HUD { get; private set; }
        [Inject] public PlayerInputService Input { get; private set; }
        [Inject, HideInInspector] public Stage stage;
        public PlayerMovementConfig Config;
        
        private void Awake()
        {
            Camera?.Init(this);
            Effects?.Init(this);
            Sounds?.Init(this);
            Model?.Init(this);
            RingUpgrade?.Init(this);
            Life?.Init(this);
            Flags?.Init(this);
            
            fsm = new PlayerFSM();
            fsm.Init(GetComponent<Rigidbody>()); // it's the player fsm so we have to get the rigidbody
            InitStates();
            fsm.SetState<StateGround>();
            fsm.LastState = fsm.CurrentState;

            var rb = fsm.rb;
            Debug.Log(rb.solverIterations);
            rb.sleepThreshold = 0;
            rb.solverIterations = 30;
        }

        /// <summary>
        /// Init all states of the actor
        /// </summary>
        protected virtual void InitStates()
        {
            fsm.AddState(new StateIdle(fsm, gameObject));
            fsm.AddState(new StateStart(fsm, gameObject, Config));
            fsm.AddState(new StateGround(fsm, gameObject, Config));
            fsm.AddState(new StateAir(fsm, gameObject, Config));
            fsm.AddState(new StateAuto(fsm, gameObject, Config));
            fsm.AddState(new StateDashRing(fsm, gameObject, Config));
            fsm.AddState(new StateJump(fsm, gameObject, Config));
            fsm.AddState(new StateSpring(fsm, gameObject, Config));
            fsm.AddState(new StateDamage(fsm, gameObject, Config));
            fsm.AddState(new StateStartGoal(fsm, gameObject, Config));
            fsm.AddState(new StateGoal(fsm, gameObject, Config));
        }

        private void Update()
        {
            fsm.FrameUpdate();
            fsm.BaseUpdate();
        }

        private void FixedUpdate()
        {
            fsm.PhysicsUpdate();
        }

        private void OnGUI()
        {
            var style = new GUIStyle();
            style.fontSize = 36;
            style.normal.textColor = Color.white;
            
            GUI.Label(new Rect(10, 10, 200, 30), $"Position: {transform.position}", style);
            GUI.Label(new Rect(10, 50, 200, 30), $"Rotation: {transform.rotation.eulerAngles}", style);
            GUI.Label(new Rect(10, 90, 200, 30), $"Velocity: {fsm.rb.velocity.magnitude}", style);
            GUI.Label(new Rect(10, 130, 200, 30), $"SqrVelocity: {fsm.rb.velocity.sqrMagnitude}", style);
            GUI.Label(new Rect(10, 170, 200, 30), 
                $"Player State: {fsm.CurrentState.ToString().Replace("SonicFramework.PlayerStates.", "")}",
                style);
            GUI.Label(new Rect(10, 210, 200, 30), 
                $"Camera State {Camera.fsm.CurrentState.ToString().Replace("SonicFramework.CameraStates.", "")}",
                style);
            
            GUI.Label(new Rect(10, 250, 200, 30), $"Flags: {string.Join(", ", Flags.List)}", style);
        }
    }
}