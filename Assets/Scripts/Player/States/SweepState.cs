using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class SweepState : BaseState
    {
        public override void Enter()
        {
            ReorientToLockOnOrMove();

            characterController.DisableInputs();
        }

        public override void Move()
        {
            audioController.PlaySweep();

            characterController.OverrideVelocity(new Vector3(0f, 10f, 0f), false);
        }
    }
}
