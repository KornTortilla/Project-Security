using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class Hitbox : MonoBehaviour
    {
        private DamageInfo damageInfo;

        public void Initialize(DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy") return;

            Debug.Log("Triggering");

            EntityHealth entityHealth = other.GetComponent<EntityHealth>();

            if (!entityHealth) return;

            entityHealth.TakeDamage(damageInfo.damage);
        }
    }
}