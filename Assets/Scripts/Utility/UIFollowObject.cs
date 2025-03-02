using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class UIFollowObject : MonoBehaviour
    {
        private GameObject target;

        public void SetTarget(GameObject gameObject)
        {
            target = gameObject;
        }

        private void Update()
        {
            transform.position = target.transform.position;
        }
    }
}
