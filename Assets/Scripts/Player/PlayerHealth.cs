using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class PlayerHealth : EntityHealth
    {
        private PlayerStateMachine playerStateMachine;

        private void Awake()
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            playerStateMachine.Hurt();
        }

        public void TakeNonKnockbackDamage(float damage)
        {
            base.TakeDamage(damage);
        }
    }
}
