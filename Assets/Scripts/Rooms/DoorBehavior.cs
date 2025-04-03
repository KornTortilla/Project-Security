using System;
using System.Collections;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField] private float delay = 5f;
    bool processingClose = true;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        Debug.Log(other);
        OpenDoor();
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (processingClose) {
            StopCoroutine(CloseDoor());
            processingClose = false;
            meshRenderer.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(CloseDoor());
    }

    private void OpenDoor()
    {
        meshRenderer.enabled = false;
        Vector3 newPos = transform.position;
        newPos.y -= 4;
        transform.position = newPos;
    }

    private IEnumerator CloseDoor() {
        yield return new WaitForSeconds(delay);
        if (!processingClose) yield return null;

        Vector3 newPos = transform.position;
        newPos.y += 4;
        transform.position = newPos;
        meshRenderer.enabled = true;
    }
}
