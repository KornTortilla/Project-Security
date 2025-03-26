using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class AttackManagerState : BaseState
    {
        private BasePlayerAttackData[] attackDatas;

        private float[] verticalDistanceRange = new float[] { -0.5f, 2 };

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
                hitboxController.StopCurrentHitbox();
                PlayNewAttack();
            }
        }

        public override void Move()
        {
            float moveAddition = 0f;
            if (!characterController.IsGrounded)
                moveAddition = Mathf.Clamp(lockOnController.GetVerticalMagnitudeToTarget(), verticalDistanceRange[0], verticalDistanceRange[1]);

            characterController.OverrideVelocity(attackDatas[index].movementVector + new Vector3(0f, moveAddition, 0f), true);
        }
    }
}