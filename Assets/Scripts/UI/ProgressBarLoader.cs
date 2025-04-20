using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBarLoader : MonoBehaviour
{
    [SerializeField] private float[] progressIncrements;

    private Slider slider;
    private int index;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Fill()
    {
        slider.value += progressIncrements[index];

        index++;
    }
}
