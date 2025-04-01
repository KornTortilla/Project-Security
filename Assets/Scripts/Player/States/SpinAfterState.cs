using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class SpinAfterState : BaseState
    {
        private float timer;
        private Vector3 velocity;

        private float exitTime = 0.6f;

        public override void Enter()
        {
            timer = 0f;

            exitTime /= speed;

            velocity = -characterController.CharacterForward * 5f;
            velocity.y = 8f;

            characterController.OverrideVelocity(velocity, false);
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (timer >= exitTime)
                Exit();
        }
    }
}
