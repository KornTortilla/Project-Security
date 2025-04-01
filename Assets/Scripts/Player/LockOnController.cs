using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace ProjectSecurity.Gameplay
{
    public class LockOnController : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private GameObject lockOnCameraObject;
        [SerializeField] private GameObject lockOnSpriteObject;
        [SerializeField] private CinemachineTargetGroup targetGroup;

        [Header("Settings")]
        [SerializeField] private float maxDistanceToTarget = 15f;
        [SerializeField] private float thresholdAngleToSuperseed = 30f;

        private InputBank inputBank;

        public bool isLockedOn = false;
        private bool triedSwitch = false;
        
        private GameObject target;

        private void Awake()
        {
            inputBank = GetComponent<InputBank>();
        }

        public void OnLockOn(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            if (!isLockedOn)
            {
                bool foundTarget = FindNearestTarget();

                if (foundTarget)
                    SetLockOn(true);
            }
            else
            {
                SetLockOn(false);
            }
        }

        private void SetLockOn(bool willBeLockedOn)
        {
            this.isLockedOn = willBeLockedOn;
            lockOnCameraObject.SetActive(willBeLockedOn);
            lockOnSpriteObject.SetActive(willBeLockedOn);
        }

        private bool FindNearestTarget()
        {
            if (EnemyHealth.readOnlyLockOnObjectList == null) return false;

            target = null;
            foreach (GameObject lockOnObject in EnemyHealth.readOnlyLockOnObjectList)
            {
                if (GetDistanceBetweenObject(gameObject, lockOnObject) > maxDistanceToTarget)
                    continue;

                if (!target)
                    target = lockOnObject;
                else if(GetDistanceBetweenObject(gameObject, lockOnObject) < GetDistanceBetweenObject(gameObject, target))
                    target = lockOnObject;
            }

            if(target)
            {
                lockOnSpriteObject.GetComponent<UIFollowObject>().SetTarget(target);
                targetGroup.m_Targets[1].target = target.transform;
                return true;
            }  
            else
            {
                return false;
            }
        }

        private void FindDirectionTarget(Vector3 direction)
        {
            if (EnemyHealth.readOnlyLockOnObjectList == null) return;

            GameObject newTarget = null;
            foreach (GameObject lockOnObject in EnemyHealth.readOnlyLockOnObjectList)
            {
                if (lockOnObject == target) continue;

                if (GetDistanceBetweenObject(gameObject, lockOnObject) > maxDistanceToTarget)
                    continue;

                if (!newTarget)
                    newTarget = lockOnObject;

                float angleToDirectionOfNew = GetAngleBetweenDirectionAndFromTarget(direction, lockOnObject);
                float angleToDirectionOfConsidered = GetAngleBetweenDirectionAndFromTarget(direction, newTarget);

                Debug.Log("New Lock On Object: " + lockOnObject.name);
                Debug.Log("Considered Lock On Object: " +newTarget.name);
                Debug.Log("Angle of new: " + angleToDirectionOfNew);
                Debug.Log("Angle of considered: " + angleToDirectionOfConsidered);

                if (angleToDirectionOfNew < angleToDirectionOfConsidered - thresholdAngleToSuperseed)
                {
                    Debug.Log("Angle low enough.");
                    newTarget = lockOnObject;
                }
                else if(angleToDirectionOfNew < angleToDirectionOfConsidered + thresholdAngleToSuperseed)
                {
                    Debug.Log("Within threshold.");
                    if (GetDistanceBetweenObject(target, lockOnObject) < GetDistanceBetweenObject(target, newTarget))
                    {
                        Debug.Log("Closer.");
                        newTarget = lockOnObject;
                    }
                }
            }

            if (newTarget)
            {
                target = newTarget;
                lockOnSpriteObject.GetComponent<UIFollowObject>().SetTarget(target);
                targetGroup.m_Targets[1].target = target.transform;
            }
        }

        private void Update()
        {
            if(isLockedOn)
            {
                if((target && GetDistanceBetweenObject(gameObject, target) > maxDistanceToTarget) || !target.activeSelf)
                    Toggle();

                if (inputBank.LockOnSwitchHeld && inputBank.CameraLookInput.magnitude > 0f && !triedSwitch)
                {
                    triedSwitch = true;
                    FindDirectionTarget(inputBank.CameraLookInput);
                }
                else if (inputBank.CameraLookInput.magnitude == 0f)
                    triedSwitch = false;
            }
        }

        private float GetDistanceBetweenObject(GameObject origin, GameObject end)
        {
            return (end.transform.position - origin.transform.position).magnitude;
        }

        public Vector3 GetDirectionToTarget()
        {
            Vector3 direction = target.transform.position - this.transform.position;
            direction.y = 0f;

            return direction.normalized;
        }

        public float GetVerticalMagnitudeToTarget()
        {
            if (target == null) return 0f;

            return target.transform.position.y - transform.position.y;
        }

        private float GetAngleBetweenDirectionAndFromTarget(Vector3 direction, GameObject newTarget)
        {
            return Vector3.Angle(direction, newTarget.transform.position - target.transform.position);
        }
    }
}
