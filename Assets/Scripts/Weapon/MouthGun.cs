using UnityEngine;
using UnityEngine.Pool;

public class MouthGun : Weapon
{
    [SerializeField] private SimpleProjectile projectilePrefab;
    [SerializeField] private float muzzleVelocity = 700f;
    [SerializeField] private Transform muzzlePosition;
    [SerializeField] private float cooldownWindow = 0.1f;

    private IObjectPool<SimpleProjectile> projectilePool;

    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private float nextTimeToShoot;

    private void Awake() {
        projectilePool = new ObjectPool<SimpleProjectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    private void FixedUpdate()
    {
        if (Time.time > nextTimeToShoot && projectilePool != null) {
            SimpleProjectile projectileObj = projectilePool.Get();

            if (projectileObj == null) return;

            projectileObj.transform.SetPositionAndRotation(muzzlePosition.position, muzzlePosition.rotation);

            projectileObj.GetComponent<Rigidbody>().AddForce(projectileObj.transform.forward * muzzleVelocity, ForceMode.Acceleration);

            projectileObj.Deactivate();

            nextTimeToShoot = Time.time + cooldownWindow;
        }
    }

    private SimpleProjectile CreateProjectile() 
    {
        SimpleProjectile projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.ObjectPool = projectilePool;
        return projectileInstance;
    }

    private void OnGetFromPool(SimpleProjectile pooledObject) {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(SimpleProjectile pooledObject) {
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(SimpleProjectile pooledObject) {
        Destroy(pooledObject.gameObject);
    }    
}
