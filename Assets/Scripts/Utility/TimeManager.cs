using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager main;

    private Coroutine freezeCoroutine;

    private bool frozen = false;
    public bool Frozen
    {
        get
        {
            return frozen;
        }
    }

    private void Awake()
    {
        if (!main)
            main = this;
        else
            Destroy(gameObject);
    }

    public void Freeze(float time)
    {
        if (frozen) return;

        frozen = true;
        freezeCoroutine = StartCoroutine(FreezeCoroutine(time));
    }

    [ContextMenu("Freeze 1s")]
    public void TestFreeze()
    {
        Freeze(1f);
    }

    private IEnumerator FreezeCoroutine(float time)
    {
        float originalScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = originalScale;
        frozen = false;
    }
}
