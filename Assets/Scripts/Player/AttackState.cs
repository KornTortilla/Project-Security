using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class AttackState : BaseState
    {
        private BasePlayerAttackData attackData;

        public AttackState(BasePlayerAttackData attackData)
        {
            this.attackData = attackData;
        }

        public override void Enter()
        {
            animator.Play(attackData.animationName);

            characterController.DisableInputs();
            characterController.OverrideVelocity(new Vector3(5f, 3f, 0f), true);

            stateMachine.hitboxController.InitalizeCurrentHitbox(attackData.damageInfo);
        }
    }
}
