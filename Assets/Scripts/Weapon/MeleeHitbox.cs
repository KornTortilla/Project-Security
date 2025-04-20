using UnityEngine;
using ProjectSecurity.Gameplay;

public class MeleeHitbox : Weapon
{
    Collider hitboxCollider;

    private void Awake() {
        hitboxCollider = GetComponentsInChildren<Collider>()[0];
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<EntityHealth>().TakeDamage(DamageInfo);
    }
}
