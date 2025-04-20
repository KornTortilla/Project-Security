using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class PlayerHealth : EntityHealth
    {
        [SerializeField] private SliderUI healthUI;

        private PlayerStateMachine playerStateMachine;
        private PlayerCharacterController playerCharacterController;

        private void Awake()
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
            playerCharacterController = GetComponent<PlayerCharacterController>();
        }

        public void TakeNonKnockbackDamage(float damage)
        {
            base.TakeDamage(damage);
        }

        public override void TakeDamage(DamageInfo damageInfo)
        {
            base.TakeDamage(damageInfo);

            if(damageInfo.damageType == DamageType.heavy)
            {
                playerStateMachine.Hurt();
                playerCharacterController.OverrideVelocity(damageInfo.knockbackVector, false);
            }

            healthUI.Update((int)CurrentHealth);
        }
    }
}
