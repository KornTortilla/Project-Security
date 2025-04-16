using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class IdleState : BaseState
    {
        public override void Enter()
        {
            changeStateOnLand = true;

            characterController.EnableInputs();
        }

        public override void Update()
        {
            if(characterController.hasJumpedThisFrame)
                animator.Play("JumpSquat");

            /*
            if (characterController.hasHitGroundThisFrame)
                animator.Play("Land");
            */
                

            if (characterController.IsGrounded)
            {
                if (inputBank.CameraMoveInput.magnitude > 0f)
                {
                    animator.Play("Idle");

                    audioController.PlayWalk();
                }
                else
                {
                    audioController.StopWalk();
                }

                animator.SetFloat("Ground Speed", characterController.Velocity.magnitude);
            }
            else if(!characterController.IsGrounded)
            {
                audioController.StopWalk();

                animator.Play("Airborne");
                animator.SetFloat("Vertical Speed", characterController.Velocity.y);
            }

            base.Update();
        }

        public override void Exit()
        {
            audioController.StopWalk();

            base.Exit();
        }
    }
}
