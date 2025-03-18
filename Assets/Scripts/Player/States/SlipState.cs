using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class SlipState : BaseState
    {
        private float timer;
        private float velocity;

        private float velocityTime = 1.2f;

        private bool canMove = false;

        public override void Enter()
        {
            timer = 0f;

            ReorientToLockOnOrMove();

            characterController.DisableEnemyCollision();
            characterController.DisableInputs();
        }

        public override void Update()
        {
            if (timer <= velocityTime && canMove)
            {
                velocity = Mathf.Lerp(12f, 0, timer / velocityTime);

                characterController.OverrideHorizontalVelocity(velocity);

                timer += Time.deltaTime;
            }
        }

        public override void Move()
        {
            Debug.Log("Movin.");

            canMove = true;
        }

        public override void Exit()
        {
            characterController.EnableEnemyCollision();

            base.Exit();
        }
    }
}
