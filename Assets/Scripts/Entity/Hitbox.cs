using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class Hitbox : MonoBehaviour
    {
        private event Action OnHit;

        private float damage;
        private Vector3 knockbackVector;

        public void Initialize(float damage, Vector3 knockbackVector)
        {
            this.damage = damage;
            this.knockbackVector = knockbackVector;
        }

        // Used by classes that deploy hitboxes, to check their own to see if they hit
        public void AddOnHitListener(Action func)
        {
            OnHit += func;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy") return;

            OnHit?.Invoke();

            // Will check for multiple variations of health for now
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(damage, knockbackVector);
                return;
            }

            EntityDamage entityDamage = other.GetComponent<EntityDamage>();
            if (entityDamage)
            {
                entityDamage.TakeDamage(damage, knockbackVector);
                return;
            }
        }
    }
}