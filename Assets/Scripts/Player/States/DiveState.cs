using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class DiveState : BaseState
    {
        private bool canMove = false;

        private float dualSpeed = 10f;

        public override void Enter()
        {
            ReorientToLockOnOrMove();

            characterController.DisableInputs();
            characterController.DisableEnemyCollision();

            dualSpeed *= speed;

            if (characterController.IsGrounded)
                characterController.OverrideVelocity(new Vector3(0f, 8f, 0), false);
            else
            {
                characterController.GravityLockout();
                characterController.OverrideVelocity(new Vector3(0f, 1f, 0), false);
            }
        }

        public override void Update()
        {
            if(canMove)
                characterController.OverrideVelocity(new Vector3(dualSpeed, -dualSpeed, 0), true);

            if (characterController.hasHitGroundThisFrame)
            {
                characterController.DisableEnemyCollision();
                stateMachine.Land();
            }
        }

        public override void Move()
        {
            canMove = true;
        }
    }
}
