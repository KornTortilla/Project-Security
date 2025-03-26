using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class LandState : BaseState
    {
        public override void Enter()
        {
            animator.Play("Land", -1, 0);

            characterController.DisableInputs();
        }
    }
}
