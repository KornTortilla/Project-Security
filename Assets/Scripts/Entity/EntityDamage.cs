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

        public void TakeDamage(float damage, Vector3 knockbackVector)
        {
            Debug.Log(knockbackVector);

            rigidbody.linearVelocity = knockbackVector;
        }
    }
}