using UnityEngine;
using UnityEngine.UI;

namespace ProjectSecurity.Gameplay
{
    public class SliderUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void Update(int amount)
        {
            slider.value = amount;
        }
    }
}
