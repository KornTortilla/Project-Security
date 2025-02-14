using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class OscillatorState : BaseState
    {
        private ActionData actionData;

        public override void Enter()
        {
            characterController.DisableInputs();
            // characterController.OverrideVelocity(0f);
        }

        public override void Update()
        {
            characterController.OverrideVelocity(2f);
        }
    }
}
