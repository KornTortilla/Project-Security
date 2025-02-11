using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : EntityHealth
{
    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    [Header("Testing Purposes")]
    public bool KnockBack;
    public float DamageRecieved;
    public float KnockBackForce;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();    
    }

    protected override void Update() {
        base.Update();
        // For testing purposes
        if (KnockBack) {
            KnockBack = false;
            TakeDamage(DamageRecieved, transform.forward * KnockBackForce);
        }
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
        Debug.Log("Enemy has been defeated!");
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
}
