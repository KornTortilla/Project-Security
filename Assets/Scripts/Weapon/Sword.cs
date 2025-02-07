using UnityEngine;

public class Sword : Weapon
{
    Collider bladeCollider;

    private void Awake() {
        bladeCollider = GetComponentsInChildren<Collider>()[0];
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        Debug.Log($"Sword collided with {other.gameObject.name}");
        other.GetComponent<EntityHealth>().TakeDamage(Damage);
    }
}
