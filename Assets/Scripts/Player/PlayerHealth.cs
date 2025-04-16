using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class PlayerHealth : EntityHealth
    {
        private PlayerStateMachine playerStateMachine;
        private PlayerCharacterController playerCharacterController;

        private void Awake()
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
            playerCharacterController = GetComponent<PlayerCharacterController>();
        }

        public void TakeDamage(float damage, Vector3 knockbackVector)
        {
            base.TakeDamage(damage);

            playerStateMachine.Hurt();
            playerCharacterController.OverrideVelocity(knockbackVector, false);
        }

        public void TakeNonKnockbackDamage(float damage)
        {
            base.TakeDamage(damage);
        }
    }
}
