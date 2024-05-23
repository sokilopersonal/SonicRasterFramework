using SonicFramework.PlayerStates;

namespace SonicFramework.Actors
{
    public class Sonic : PlayerBase
    {
        protected override void InitStates()
        {
            base.InitStates();
            
            fsm.AddState(new StateDropDash(fsm, gameObject, Config));
            fsm.AddState(new StateDropDashForce(fsm, gameObject, Config));
            fsm.AddState(new StateRoll(fsm, gameObject, Config));
            fsm.AddState(new StateJumpDash(fsm, gameObject, Config));
            fsm.AddState(new StateHoming(fsm, gameObject, Config));
            fsm.AddState(new StateAfterHoming(fsm, gameObject, Config));
            fsm.AddState(new StateSpinDashCharge(fsm, gameObject, Config));
            fsm.AddState(new StateLightSpeedDash(fsm, gameObject, Config));
        }
    }
}