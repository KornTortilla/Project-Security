using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class SpinIntroState : BaseState
    {
        private float timer;

        private float exitTime = 2f;

        private bool canMove = false;

        public override void Enter()
        {
            timer = 0f;

            ReorientToLockOnOrMove();

            characterController.DisableInputs();
        }

        public override void Update()
        {
            if(canMove)
                characterController.OverrideVelocity(10f);

            timer += Time.deltaTime;

            if (timer >= exitTime)
                Exit();
        }

        public override void Move()
        {
            canMove = true;

            ReorientToLockOnOrMove();
        }

        public override void HandleHitboxHit()
        {
            stateMachine.SetState(new SpinAfterState());
        }

        public override void Exit()
        {
            animator.Play("Idle");

            base.Exit();
        }
    }
}
