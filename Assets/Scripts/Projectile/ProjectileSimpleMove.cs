using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class ProjectileSimpleMove : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private Vector3 direction = Vector3.forward;

        public void SetMoveDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
