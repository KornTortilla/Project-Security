using UnityEngine;

public class Sword : MonoBehaviour
{
    Collider bladeCollider;

    private void Awake() {
        bladeCollider = GetComponentsInChildren<Collider>()[0];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        Debug.Log($"Sword collided with {other.gameObject.name}");
        other.GetComponent<EntityHealth>().TakeDamage(10);
    }
}
