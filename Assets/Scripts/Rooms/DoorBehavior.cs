using System;
using System.Collections;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField] private float delay = 5f;
    [SerializeField] private bool doorOpenableOnStart = true;
    public bool StayOpen {get; private set;} = false;
    MeshRenderer meshRenderer;
    BoxCollider openDoorCollider;
    bool processingClose = true;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        openDoorCollider = GetComponent<BoxCollider>();
    }

    private void Start() {
        openDoorCollider.enabled = doorOpenableOnStart;
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
        if (!StayOpen) {
            yield return new WaitForSeconds(delay);
            if (!processingClose) yield return null;

            Vector3 newPos = transform.position;
            newPos.y += 4;
            transform.position = newPos;
            meshRenderer.enabled = true;
        }
    }

    public void PermaOpenDoor() {
        OpenDoor();
        StayOpen = true;
    }

    public void EnableOpenDoorCollider() {
        openDoorCollider.enabled = true;
    }
}
