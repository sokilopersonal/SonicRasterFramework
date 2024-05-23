namespace SonicFramework.StateMachine
{
    public abstract class State
    {
        protected readonly FSM fsm;
        
        public State(FSM fsm)
        {
            this.fsm = fsm;
        }
        
        public virtual void Enter() 
        {
            
        }
        
        public virtual void FrameUpdate() 
        {
            
        }
        
        public virtual void PhysicsUpdate() 
        {
            
        }

        public virtual void OnGizmos()
        {
            
        }
        
        public virtual void Exit()
        {
            fsm.LastState = this;
        }

        public abstract void BaseUpdate();
    }
}
