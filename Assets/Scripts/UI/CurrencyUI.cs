using UnityEngine;
using TMPro;

namespace ProjectSecurity.Gameplay
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI heapTMP;

        public void UpdateHeap(int amount)
        {
            heapTMP.text = amount.ToString();
        }
    }
}
