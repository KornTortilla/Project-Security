using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private CurrencyUI currencyUI;

        private int heap;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Drop") return;

            heap++;
            currencyUI.UpdateHeap(heap);
        }
    }
}