using System;
using System.Collections.Generic;
using UnityEngine;

namespace SonicFramework.StateMachine
{
    public class FSM
    {
        public State CurrentState { get; protected set; }
        public State LastState { get; set; }

        public Dictionary<Type, State> states = new();

        public Action<State> OnStateChanged;
        public Action<State> OnStateEnter;
        public Action<State> OnStateExit;

        public void AddState(State state)
        {
            states.Add(state.GetType(), state);
        }

        public virtual void SetState<T>() where T : State
        {
            var type = typeof(T);

            if (CurrentState != null && CurrentState.GetType() == type)
            {
                return;
            }

            if (states.TryGetValue(type, out var newState))
            {
                LastState = CurrentState;
                OnStateExit?.Invoke(CurrentState);
                CurrentState?.Exit();
                CurrentState = newState;
                OnStateChanged?.Invoke(CurrentState);
                CurrentState?.Enter();
                OnStateEnter?.Invoke(CurrentState);
            }
        }

        public bool GetState<T>(out T state) where T : State
        {
            var type = typeof(T);
            if (states.TryGetValue(type, out var newState))
            {
                state = (T)newState;
                return true;
            }

            state = null;
            return false;
        }

        public T GetState<T>() where T : State
        {
            var type = typeof(T);
            if (states.TryGetValue(type, out var newState))
            {
                return (T)newState;
            }
            
            return null;
        }

        public void FrameUpdate()
        {
            CurrentState?.FrameUpdate();
        }

        public void BaseUpdate()
        {
            CurrentState?.BaseUpdate();
        }

        public void PhysicsUpdate()
        {
            CurrentState?.PhysicsUpdate();
        }
    }
}
