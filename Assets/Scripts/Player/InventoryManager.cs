using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private CurrencyUI currencyUI;

        private ActionController actionController;

        private int heap = 50;
        public int Heap
        {
            get { return heap; }
            set
            {
                currencyUI.UpdateHeap(value);
                heap = value;
            }
        }

        private void Awake()
        {
            actionController = GetComponent<ActionController>();
            currencyUI.UpdateHeap(heap);
        }

        public void GetNewSpecialAction(SpecialAction specialAction)
        {
            actionController.AddSpecialAction(specialAction);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Drop") return;

            Heap++;
        }
    }
}