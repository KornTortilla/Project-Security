using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("You finished the level!");
    }
}
