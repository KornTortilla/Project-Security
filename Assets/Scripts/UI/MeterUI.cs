using UnityEngine;
using UnityEngine.UI;

namespace ProjectSecurity.Gameplay
{
    public class MeterUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void UpdateMeter(int amount)
        {
            slider.value = amount;
        }
    }
}
