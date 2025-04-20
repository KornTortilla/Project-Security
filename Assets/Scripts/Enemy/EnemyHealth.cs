using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectSecurity.Gameplay;
using Unity.Behavior;
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

    private Coroutine enableNavMeshCoroutine;

    private static readonly List<GameObject> lockOnObjectList = new List<GameObject>();
    public static readonly ReadOnlyCollection<GameObject> readOnlyLockOnObjectList = lockOnObjectList.AsReadOnly();

    [Header("BehaviorTree")]
    BehaviorGraphAgent behaviorAgent;

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
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
    }

    protected override void Start() {
        base.Start();
        // behaviorAgent = GetComponent<BehaviorGraphAgent>();
        
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
        // Drop Loot
        Debug.Log("Enemy has been defeated!");
        if (heapObj) {
            heapObj.transform.position = transform.position;
            heapObj.SetActive(true);
        }
        if (stackObj) {
            stackObj.transform.position = transform.position;
            stackObj.SetActive(true);
        }

        // Object pool management
        if(objectPool != null) objectPool.Release(this);
        else gameObject.SetActive(false);

        // Message Room Clear Condition
        SendMessageUpwards("DecreaseEnemyCount");
    }
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log(name + " has taken " + damage + " damage!");
        behaviorAgent.BlackboardReference.SetVariableValue("EnemyHealthValue",CurrentHealth);
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    
    public void TakeDamage(float damage, Vector3 knockBackForce)
    {
        CurrentHealth -= damage;
        Debug.Log(name + " has taken " + damage + " damage!");
        behaviorAgent.BlackboardReference.SetVariableValue("EnemyHealthValue",CurrentHealth);
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        navMeshAgent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.linearVelocity = knockBackForce;
        Debug.Log("Knockback Vector: " + knockBackForce);
        Debug.Log($"Linear Velocity: {rb.linearVelocity}");

        if (enableNavMeshCoroutine != null)
            StopCoroutine(enableNavMeshCoroutine);
        enableNavMeshCoroutine = StartCoroutine(EnableNavMesh());
    }

    public override void TakeDamage(DamageInfo damageInfo)
    {
        CurrentHealth -= damageInfo.damage;
        behaviorAgent.BlackboardReference.SetVariableValue("EnemyHealthValue",CurrentHealth);
        Debug.Log("Ouch! I only have " + CurrentHealth + " health left!");

        if (CurrentHealth <= 0)
        {
            Die();
            return;
        }

        navMeshAgent.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if(rb.linearVelocity.magnitude <= damageInfo.knockbackVector.magnitude / 2f)
            rb.linearVelocity = damageInfo.knockbackVector;
        Debug.Log("Knockback Vector: " + damageInfo.knockbackVector);
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
