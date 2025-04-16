using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class HurtState : BaseState
    {
        private float timer;

        private float exitTime = 0.6f;

        public override void Enter()
        {
            timer = 0f;

            if(characterController.IsGrounded)
                animator.Play("HurtGround", -1, 0f);
            else
                animator.Play("HurtAir", -1, 0f);

            characterController.DisableInputs();

            Vector3 newDirection = characterController.Velocity;
            newDirection.x *= -1;
            newDirection.y = 0;
            newDirection.z *= -1;

            characterController.OverrideRotation(newDirection.normalized);
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= exitTime)
                stateMachine.SetStateToDefault();
        }
    }
}
