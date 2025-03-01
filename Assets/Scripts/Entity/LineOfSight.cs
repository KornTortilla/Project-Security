using System;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField][Min(0)]
    float radius = 5f;
    [SerializeField][Range(0, 180)][Tooltip("The field of view of the entity on one side")]
    private float halfFOV = 45f;
    private bool playerInSight = false;
    public GameObject Player {get; private set;} = null;

    public bool PlayerIsInSight()
    {
        DetectPlayer();
        if (Player != null && PlayerIsWithinView())
        {
            Physics.Raycast(transform.position, (Player.transform.position - transform.position).normalized, out RaycastHit hit, radius);
            // Debug.Log("Hit: " + hit);
            playerInSight = hit.collider != null && hit.collider.gameObject == Player;
        }
        else
        {
            playerInSight = false;
        }
        return playerInSight;
    }

    private void DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            Player = colliders[0].gameObject;
        }
    }

    private bool PlayerIsWithinView()
    {
        Vector3 vecA = transform.forward;
        vecA.y = 0;
        Vector3 vecB = Player.transform.position - transform.position;
        vecB.y = 0;
        vecB.Normalize();
        float angle = Vector3.Angle(vecA, vecB);
        return angle < halfFOV;
    }

    private void OnDrawGizmos() {
        if (Player == null) return;
        Physics.Raycast(transform.position, (Player.transform.position - transform.position).normalized, out RaycastHit hit, radius);
        if (hit.collider != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
        }
        else {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * radius);
        }
    }
}
    
