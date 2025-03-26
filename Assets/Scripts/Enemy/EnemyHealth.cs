using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    private Coroutine enableNavMeshCoroutine;

    private static readonly List<GameObject> lockOnObjectList = new List<GameObject>();
    public static readonly ReadOnlyCollection<GameObject> readOnlyLockOnObjectList = lockOnObjectList.AsReadOnly();

    private void OnEnable()
    {
        lockOnObjectList.Add(gameObject);
    }

    private void OnDisable()
    {
        lockOnObjectList.Remove(gameObject);
    }

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
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        navMeshAgent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if(rb.linearVelocity.magnitude <= knockBackForce.magnitude / 2f)
            rb.linearVelocity = knockBackForce;
        Debug.Log("Knockback Vector: " + knockBackForce);
        Debug.Log($"Linear Velocity: {rb.linearVelocity}");

        if (enableNavMeshCoroutine != null)
            StopCoroutine(enableNavMeshCoroutine);
        enableNavMeshCoroutine = StartCoroutine(EnableNavMesh());
    }

    private IEnumerator EnableNavMesh() {
        float timer = 0f;
        yield return Time.fixedDeltaTime;
        while (rb.linearVelocity.y != 0f || timer < 0.2f) {
            Debug.Log($"Linear Velocity: {rb.linearVelocity}");
            timer += Time.fixedDeltaTime;
            yield return Time.fixedDeltaTime;
        }

        navMeshAgent.enabled = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
