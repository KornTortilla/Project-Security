using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyHealth : EntityHealth
{
    NavMeshAgent navMeshAgent;
    Rigidbody rb;
    [Header("Testing Purposes")]
    public bool KnockBack;
    public float DamageRecieved;
    public float KnockBackForce;


    [Header("ObjectPooling")]
    [SerializeField] protected float timeoutDelay = 3f;
    protected IObjectPool<EnemyHealth> objectPool;
    public IObjectPool<EnemyHealth> ObjectPool { set => objectPool = value; }

    [Header("Drop Objects")]
    [SerializeField] GameObject heapObj;
    [SerializeField] GameObject stackObj;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();    
    }

    protected override void Start() {
        base.Start();
        if (heapObj) heapObj.SetActive(false);
        if (stackObj) stackObj.SetActive(false);
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
        Debug.Log("Enemy has been defeated!");
        if (heapObj) {
            heapObj.transform.position = transform.position;
            heapObj.SetActive(true);
        }
        if (stackObj) {
            stackObj.transform.position = transform.position;
            stackObj.SetActive(true);
        }

        if(objectPool != null) objectPool.Release(this);
        else gameObject.SetActive(false);
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
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
