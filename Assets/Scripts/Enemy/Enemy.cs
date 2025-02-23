using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float timeoutDelay = 3f;
    protected IObjectPool<Enemy> objectPool;
    public IObjectPool<Enemy> ObjectPool { set => objectPool = value; }

    public void ReleaseEnemy()
    {
        objectPool.Release(this);
    }
}
