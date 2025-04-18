using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class AttackManagerState : BaseState
    {
        private BasePlayerAttackData[] attackDatas;

        private float[] verticalDistanceRange = new float[] { -0.5f, 2 };
        private float horizontalThreshold = 2f;

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
            Vector3 moveVector = attackDatas[index].movementVector;

            if(lockOnController.isLockedOn) 
            {
                Vector3 horiVector = Vector3.ProjectOnPlane(moveVector, Vector3.up);
                // Debug.Log(horiVector);
                // Debug.Log(horiVector.magnitude);
                float magnitudeToTarget = lockOnController.GetHorizontalMagnitudeToTarget();
                if (magnitudeToTarget < horizontalThreshold)
                    horiVector *= Mathf.Lerp(1f, 0.2f, magnitudeToTarget / horizontalThreshold);

                // Debug.Log(horiVector.magnitude);

                float addition = 0f;
                if (!characterController.IsGrounded)
                    addition = Mathf.Clamp(lockOnController.GetVerticalMagnitudeToTarget(), verticalDistanceRange[0], verticalDistanceRange[1]);

                moveVector = new Vector3(horiVector.x, moveVector.y + addition, horiVector.z);
            }

            characterController.OverrideVelocity(moveVector, true);
        }
    }
}