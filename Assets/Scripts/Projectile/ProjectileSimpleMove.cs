using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class ProjectileSimpleMove : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float timeAlive = 5f;

        private Vector3 direction = Vector3.forward;
        private float timer = 0f;

        public void SetMoveDirection(Vector3 direction)
        {
            this.direction = direction;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += direction * speed * Time.deltaTime;

            timer += Time.deltaTime;

            if (timer > timeAlive)
            {
                GetComponent<AkEvent>().Stop(0);
                Destroy(gameObject);
            }
        }
    }
}
