using System;
using SonicFramework.StateMachine;

namespace SonicFramework
{
    public class CameraFSM : FSM
    {
        public override void SetState<T>()
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
    }
}