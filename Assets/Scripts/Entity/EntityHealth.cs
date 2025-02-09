using UnityEngine;

public abstract class EntityHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    protected float maxHealth = 100f;
    public float CurrentHealth { get; protected set; }

    [Header("For Testing Purposes")]
    public bool ClickToHeal = false;
    public bool ClickToDamage = false;

    protected virtual void Start()
    {
        CurrentHealth = maxHealth;
    }

    protected virtual void Update() {
        // For testing purposes
        if (ClickToHeal) {
            ClickToHeal = false;
            Heal(10);
        }
        if (ClickToDamage) {
            ClickToDamage = false;
            TakeDamage(10);
        }
    }

    public virtual  void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Heal(float healAmount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + healAmount , 0, maxHealth);
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }    
}
