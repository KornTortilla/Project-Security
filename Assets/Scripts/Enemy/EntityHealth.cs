using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    public float CurrentHealth { get; private set; }
    public bool ClickToHeal = false;
    public bool ClickToDamage = false;

    private void Start()
    {
        CurrentHealth = 10;
    }

    private void Update() {
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

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void Heal(float healAmount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + healAmount , 0, maxHealth);
    }
}
