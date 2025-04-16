using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class DashState : BaseState
    {
        private float timer;
        private float velocity;

        private float velocityTime = 0.6f;
        private float exitTime = 0.8f;

        public override void Enter()
        {
            timer = 0f;

            animator.Play("Dash", -1, 0f);
            audioController.PlayDash();

            ReorientToMove();

            characterController.DisableInputs();
        }

        public override void Update()
        {
            if(timer <= velocityTime)
            {
                velocity = Mathf.Lerp(15f, 5f, timer / velocityTime);

                characterController.OverrideVelocity(velocity);
            }

            timer += Time.deltaTime;

            if (timer >= exitTime)
                stateMachine.SetStateToDefault();
        }
    }
}
