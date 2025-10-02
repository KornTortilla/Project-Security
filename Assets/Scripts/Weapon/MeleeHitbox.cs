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

        DamageInfo damageInfo = DamageInfo;
        damageInfo.knockbackVector = RotateKnockback(damageInfo.knockbackVector);
        Debug.Log("Parent rotation: " + (transform.parent.eulerAngles.y - 90));
        other.GetComponent<EntityHealth>().TakeDamage(damageInfo);
    }

    private Vector3 RotateKnockback(Vector3 knockbackVector)
    {
        return Quaternion.AngleAxis(transform.parent.eulerAngles.y - 90, Vector3.up) * knockbackVector;
    }
}
