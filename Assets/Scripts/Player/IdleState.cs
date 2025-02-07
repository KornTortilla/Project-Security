using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class IdleState : BaseState
    {
        public override void Enter()
        {
            characterController.EnableInputs();
        }

        public override void Exit()
        {
            characterController.DisableInputs();

            base.Exit();
        }
    }
}
