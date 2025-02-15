using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] private float timeoutDelay = 3f;

    private IObjectPool<SimpleProjectile> objectPool;
    public IObjectPool<SimpleProjectile> ObjectPool { set => objectPool = value; }

    public void Deactivate() {
        StartCoroutine(DeactivateRoutine(timeoutDelay));
    }

    IEnumerator DeactivateRoutine(float delay) {
        yield return new WaitForSeconds(delay);

        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.linearVelocity = new Vector3(0f, 0f, 0f);
        rBody.angularVelocity = new Vector3(0f, 0f, 0f);

        objectPool.Release(this);
    }
}
