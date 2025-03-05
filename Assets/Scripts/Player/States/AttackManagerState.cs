using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class AttackManagerState : BaseState
    {
        private BasePlayerAttackData[] attackDatas;

        private int index = 0;

        public AttackManagerState(BasePlayerAttackData[] attackDatas)
        {
            this.attackDatas = attackDatas;
        }

        public override void Enter()
        {
            changeStateOnLand = true;

            PlayNewAttack();
        }

        private void PlayNewAttack()
        {
            animator.Play(attackDatas[index].animationName, -1, 0f);

            base.ReorientToLockOnOrMove();

            characterController.DisableInputs();
            // characterController.GravityLockout();
            characterController.OverrideVelocity(0f);

            stateMachine.hitboxController.InitalizeDefaultHitbox(attackDatas[index].damageInfo);
        }

        public void Continue()
        {
            index++;

            if (index < attackDatas.Length)
            {
                PlayNewAttack();
            }
        }

        public override void Move()
        {
            characterController.OverrideVelocity(attackDatas[index].movementVector, true);
        }
    }
}