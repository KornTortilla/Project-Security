using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemy;
    [SerializeField] private GameObject enemyPrefab;
    
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float cooldownWindow = 0.1f;

    private IObjectPool<EnemyHealth> enemyPool;

    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private float nextTimeToSpawn;

    private void Awake() {
        enemyPool = new ObjectPool<EnemyHealth>(CreateEnemy, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    private void Update() {
        Spawn();    
    }

    public void Spawn()
    {
        if (Time.time > nextTimeToSpawn && enemyPool != null) {
            EnemyHealth enemyObj = enemyPool.Get();
            if (enemyObj == null) return;
            NavMesh.SamplePosition(spawnPosition.position, out NavMeshHit hit, 5f, NavMesh.AllAreas);
            // Debug.Log("Hit Pos: " + hit.position);
            enemyObj.transform.SetPositionAndRotation(hit.position, quaternion.identity);
            nextTimeToSpawn = Time.time + cooldownWindow;
        }
    }

    private EnemyHealth CreateEnemy() 
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, transform);
        EnemyHealth enemyHealthInstance = enemyPrefab.GetComponent<EnemyHealth>();
        enemyHealthInstance.ObjectPool = enemyPool;
        return enemyHealthInstance;

    }

    private void OnGetFromPool(EnemyHealth pooledObject) {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(EnemyHealth pooledObject) {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(EnemyHealth pooledObject) {
        Destroy(pooledObject.gameObject);
    } 
}
