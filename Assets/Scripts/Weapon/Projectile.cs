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
    bool releasingProjectile = false;

    private void ReleaseProjectile()
    {
        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.linearVelocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);

        objectPool.Release(this);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (releasingProjectile) return;
        releasingProjectile = true;
        if (other.CompareTag("Player")) {
            other.GetComponent<EntityHealth>().TakeDamage(DamageInfo);
        }
        ReleaseProjectile();
    }
}
