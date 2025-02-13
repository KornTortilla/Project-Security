using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class DefaultActionState : BaseState
    {
        private ActionData actionData;

        public DefaultActionState(ActionData actionData)
        {
            this.actionData = actionData;
        }

        public override void Enter()
        {
            animator.Play(actionData.animationName);

            characterController.DisableInputs();
            characterController.OverrideVelocity(0f);
        }
    }
}

