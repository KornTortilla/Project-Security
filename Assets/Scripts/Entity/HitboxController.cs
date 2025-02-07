using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class HitboxController : MonoBehaviour
    {
        [SerializeField] private Hitbox defaultHitbox;

        public void InitalizeCurrentHitbox(DamageInfo damageInfo)
        {
            defaultHitbox.Initialize(damageInfo);
        }

        public void EnableDefaultHitbox()
        {
            defaultHitbox.gameObject.SetActive(true);
        }

        public void DisableDefaultHitbox()
        {
            defaultHitbox.gameObject.SetActive(false);
        }
    }
}