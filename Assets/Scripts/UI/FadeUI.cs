using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeUI : MonoBehaviour
{
    [SerializeField] private bool fadingIn;
    [SerializeField] private float timeToFade;

    private Image image;
    private Color startingColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        startingColor = image.color;

        if (fadingIn)
            startingColor.a = 0f;
        else
            startingColor.a = 1f;

        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float timer = 0f;

        Color endingColor = startingColor;
        if (fadingIn)
            endingColor.a = 1f;
        else
            endingColor.a = 0f;

        while (timer <= timeToFade)
        {
            image.color = Color.Lerp(startingColor, endingColor, timer/timeToFade);

            timer += Time.deltaTime;

            yield return 0f;
        }

        image.color = endingColor;
    }
}
