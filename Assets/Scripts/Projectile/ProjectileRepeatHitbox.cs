using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class ProjectileRepeatHitbox : MonoBehaviour
    {
        [SerializeField] private GameObject hitboxObject;
        [SerializeField] private AK.Wwise.Event hitEvent;
        [SerializeField] private float interval = 0.2f;

        private float timer;

        private void Start()
        {
            timer = interval;
        }

        public void SetHitDirection(Vector3 direction)
        {
            hitboxObject.GetComponent<Hitbox>().Initialize(1f, direction);

            hitboxObject.GetComponent<Hitbox>().AddOnHitListener(() => SoundOff());
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if(timer < 0f)
            {
                timer = interval;

                hitboxObject.SetActive(!hitboxObject.activeSelf);
            }
        }

        private void SoundOff()
        {
            hitEvent.Post(gameObject);
        }
    }
}