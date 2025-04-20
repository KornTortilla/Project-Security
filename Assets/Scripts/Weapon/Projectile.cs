using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using ProjectSecurity.Gameplay;

public class Projectile : MonoBehaviour
{
   [SerializeField] protected float timeoutDelay = 3f;
    protected IObjectPool<Projectile> objectPool;
    public IObjectPool<Projectile> ObjectPool { set => objectPool = value; }
    public DamageInfo DamageInfo;

    public virtual void Deactivate() {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    public virtual IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseProjectile();
    }

    private void ReleaseProjectile()
    {
        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.linearVelocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);

        objectPool.Release(this);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.GetComponent<EntityHealth>().TakeDamage(DamageInfo);
        }
        StopCoroutine(nameof(DeactivateRoutine));
        ReleaseProjectile();
    }
}
