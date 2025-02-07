using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class EntityDamage : MonoBehaviour
    {
        private Rigidbody rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            rigidbody.linearVelocity = damageInfo.knockbackVector;
        }
    }
}