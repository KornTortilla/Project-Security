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

            Debug.Log("Hehehehehehe!");

            if(damageInfo.damageType == DamageType.heavy)
            {
                playerCharacterController.OverrideVelocity(damageInfo.knockbackVector, false);
                playerStateMachine.Hurt();
            }

            healthUI.Update((int)CurrentHealth);
        }
    }
}
