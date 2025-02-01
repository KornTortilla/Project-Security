using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position, 5f, transform.forward, out RaycastHit hitInfo, 5f/*, LayerMask.GetMask("Default")*/)) {
            Debug.Log(hitInfo.collider.name);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }
}
