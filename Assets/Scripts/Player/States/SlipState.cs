using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class SlipState : BaseState
    {
        private float timer;
        private float velocity;

        private float forwardSpeed = 12f;
        private float velocityTime = 1.2f;

        private bool canMove = false;

        public override void Enter()
        {
            timer = 0f;

            ReorientToLockOnOrMove();

            characterController.DisableEnemyCollision();
            characterController.DisableInputs();

            forwardSpeed *= speed;
            velocityTime /= speed;
        }

        public override void Update()
        {
            if (timer <= velocityTime && canMove)
            {
                velocity = Mathf.Lerp(forwardSpeed, 0, timer / velocityTime);

                characterController.OverrideHorizontalVelocity(velocity);

                timer += Time.deltaTime;
            }
        }

        public override void Move()
        {
            canMove = true;
        }

        public override void Exit()
        {
            characterController.EnableEnemyCollision();

            base.Exit();
        }
    }
}
