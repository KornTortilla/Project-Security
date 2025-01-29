using System;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField][Min(0)]
    private float range = 5f;
    [SerializeField][Range(0, 180)]
    private float halfAngle = 45f;
    [SerializeField][Min(1)]
    private float angleBetweenRays = 5f;

    Dictionary<string, List<GameObject>> objectsInSight = new();

    private void Update() {
        CastLineOfSight();
        DebugDictionary();
    }

    private void CastLineOfSight() {
        Vector3 direction = transform.forward;
        direction = Quaternion.AngleAxis(-halfAngle, transform.up) * transform.forward;
        for (float i = -halfAngle; i < halfAngle; i += angleBetweenRays)
        {
            direction = Quaternion.AngleAxis(i, transform.up) * transform.forward;
            if(Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, range)) {
                if (objectsInSight.TryAdd(hitInfo.collider.tag, new List<GameObject>())) {
                    objectsInSight[hitInfo.collider.tag].Add(hitInfo.collider.gameObject);
                } else if(!objectsInSight[hitInfo.collider.tag].Contains(hitInfo.collider.gameObject)) {
                    objectsInSight[hitInfo.collider.tag].Add(hitInfo.collider.gameObject);
                }
            }
        }
    }

    private void DebugDictionary() {
        // ObjectsInSight Dict
        foreach (KeyValuePair<string, List<GameObject>> entry in objectsInSight)
        {
            Debug.Log(entry.Key + ": " + entry.Value.Count);
        }
    }

    private void OnDrawGizmosSelected() { 
        Vector3 direction = transform.forward;
        direction = Quaternion.AngleAxis(-halfAngle, transform.up) * transform.forward;
        for (float i = -halfAngle; i < halfAngle; i += angleBetweenRays)
        {
            Gizmos.color = Color.red;
            direction = Quaternion.AngleAxis(i, transform.up) * transform.forward;
            if(Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, range)) {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, direction * hitInfo.distance);
            } else {
                Gizmos.DrawRay(transform.position, direction * range);
            }
        }
    }
}
