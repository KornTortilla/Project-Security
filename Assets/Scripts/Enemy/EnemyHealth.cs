using UnityEngine;

public class EnemyHealth : EntityHealth
{
    protected override void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Enemy has been defeated!");
<<<<<<< Updated upstream
    }    
=======
    }
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage, Vector3 knockBackForce)
    {
        CurrentHealth -= damage;
        navMeshAgent.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(knockBackForce, ForceMode.Impulse);
        Debug.Log($"Linear Velocity: {rb.linearVelocity}");
        StartCoroutine(EnableNavMesh());
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator EnableNavMesh() {
        yield return Time.fixedDeltaTime;
        while (rb.linearVelocity.magnitude > 0f) {
            Debug.Log($"Linear Velocity: {rb.linearVelocity}");
            yield return Time.fixedDeltaTime;
        }
        navMeshAgent.enabled = true;
    }
>>>>>>> Stashed changes
}
