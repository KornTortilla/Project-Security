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

            if (inputBank.CameraMoveInput.magnitude > 0f)
            {
                float angle = Vector3.Angle(characterController.CharacterForward, inputBank.CameraMoveInput);
                float multiplier;
                if (angle <= 90f)
                    multiplier = angle / 90f;
                else
                    multiplier = (180f - angle) / 90f;

                Vector3 cross = Vector3.Cross(characterController.CharacterForward, inputBank.CameraMoveInput);
                if (cross.y < 0) multiplier *= -1;

                characterController.OverrideRotation(Quaternion.AngleAxis(multiplier, Vector3.up) * characterController.CharacterForward);
            }

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
