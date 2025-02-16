using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class Hitbox : MonoBehaviour
    {
        private float damage;
        private Vector3 knockbackVector;

        public void Initialize(float damage, Vector3 knockbackVector)
        {
            this.damage = damage;
            this.knockbackVector = knockbackVector;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy") return;

            EntityDamage entityDamage = other.GetComponent<EntityDamage>();

            if (!entityDamage) return;

            entityDamage.TakeDamage(damage, knockbackVector);
        }
    }
}