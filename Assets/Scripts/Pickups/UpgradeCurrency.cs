using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class UpgradeCurrency : MonoBehaviour, IPickupable
{
    [Min(0)] public int value;
    Collider collider;

    private void Awake() {
        collider = GetComponent<Collider>();
    }

    void Start()
    {
        collider.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player is picking up " + this.name);
        ProcessPickUp(other.gameObject);
    }

    public abstract void ProcessPickUp(GameObject playerObj);
}
