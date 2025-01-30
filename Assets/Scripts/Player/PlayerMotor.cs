using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;

        private int maxBounces = 5;
        private float skinWidth = 0.015f;
        private float maxSlopeAngle = 55f;

        private InputBank inputBank;
        private Collider collider;
        private Bounds bounds;

        private Vector3 moveRequest;
        private Vector3 velocity;
        private Vector3 gravity;

        private void Start()
        {
            inputBank = GetComponent<InputBank>();
            collider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            GetBounds();
            Determine();
            Move();
        }

        private void GetBounds()
        {
            bounds = collider.bounds;
            bounds.Expand(-2 * skinWidth);
        }

        private void Determine()
        {
            if (gravity.magnitude >= 9.81f * Time.fixedDeltaTime)
                return;

            gravity += Vector3.down * Time.fixedDeltaTime;
        }

        private void Move()
        {
            velocity = inputBank.CameraMoveInput * speed * Time.fixedDeltaTime;

            velocity = CollideAndSlide(velocity, transform.position, 0, false, velocity);
            velocity += CollideAndSlide(gravity, transform.position + velocity, 0, true, gravity);

            transform.position += velocity;
        }

        private Vector3 CollideAndSlide(Vector3 vel, Vector3 pos, int depth, bool gravityPass, Vector3 velInit)
        {
            if (depth >= maxBounces)
                return Vector3.zero;

            Vector3 bottomPos = pos - Vector3.up / 2f;
            Vector3 topPos = pos + Vector3.up / 2f;
            float distance = vel.magnitude + skinWidth;

            RaycastHit hit;
            if (Physics.CapsuleCast(topPos, bottomPos, bounds.extents.x, vel.normalized, out hit, distance, Physics.AllLayers))
            {
                //Debug.Log("Hehe");

                Vector3 snapToSurface = vel.normalized * (hit.distance - skinWidth);
                Vector3 leftover = vel - snapToSurface;
                float angle = Vector3.Angle(Vector3.up, hit.normal);

                if (snapToSurface.magnitude <= skinWidth)
                    snapToSurface = Vector3.zero;

                if (angle <= maxSlopeAngle)
                {
                    if (gravityPass)
                        return snapToSurface;

                    leftover = ProjectAndScale(leftover, hit.normal);
                }
                else
                {
                    // TODO: Sovle jitter against steep slopes

                    Debug.Log("Haha.");

                    float scale = 1 - Vector3.Dot(
                        new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
                        -new Vector3(velInit.x, 0 , velInit.z).normalized
                     );

                    leftover = ProjectAndScale(leftover, hit.normal) * scale;
                }

                return snapToSurface + CollideAndSlide(leftover, pos + snapToSurface, depth + 1, gravityPass, velInit);
            }

            return vel;
        }

        private Vector3 ProjectAndScale(Vector3 vec, Vector3 normal)
        {
            float magnitude = vec.magnitude;
            vec = Vector3.ProjectOnPlane(vec, normal).normalized;
            vec *= magnitude;

            return vec;
        }
    }
}
