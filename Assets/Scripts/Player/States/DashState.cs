using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class DashState : BaseState
    {
        private float timer;
        private float exitTime = 0.2f;

        public override void Enter()
        {
            timer = 0f;

            characterController.OverrideRotationToCurrentInput();
            characterController.DisableInputs();
        }

        public override void Update()
        {
            characterController.OverrideVelocity(15f);

            timer += Time.deltaTime;

            if (timer >= exitTime)
                Exit();
        }

        public override void Exit()
        {


            base.Exit();
        }
    }
}
