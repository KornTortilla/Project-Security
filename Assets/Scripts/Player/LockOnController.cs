using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectSecurity.Gameplay
{
    public class LockOnController : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject lockOnCameraObject;
        [SerializeField] private GameObject lockOnSpriteObject;

        [Header("Settings")]
        [SerializeField] private float maxDistanceToTarget = 15f;

        public bool isLockedOn = false;

        public void OnLockOn(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            isLockedOn = !isLockedOn;
            lockOnCameraObject.SetActive(isLockedOn);
            lockOnSpriteObject.SetActive(isLockedOn);

            lockOnSpriteObject.GetComponent<UIFollowObject>().SetTarget(target);
        }

        private void Update()
        {
            if(isLockedOn)
            {
                float distanceToTarget = (target.transform.position - this.transform.position).magnitude;
                if (distanceToTarget > maxDistanceToTarget)
                    Toggle();
            }
        }

        public Vector3 GetVectorToTarget()
        {
            Vector3 direction = target.transform.position - this.transform.position;
            direction.y = 0f;

            return direction.normalized;
        }
    }
}
