using System.Collections;
using UnityEngine;
using TMPro;

public class TextTyper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeBetweenTyping;
    [SerializeField] private float timeBetweenUnderscore;

    private TextMeshProUGUI textMesh;
    private string savedText;
    private int index;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(TypeCoroutine());
    }

    private IEnumerator TypeCoroutine()
    {
        savedText = textMesh.text;
        textMesh.text = "";

        while (textMesh.text != savedText)
        {
            textMesh.text += savedText[index];

            index++;

            yield return new WaitForSeconds(timeBetweenTyping);
        }

        StartCoroutine(UnderscoreCoroutine());
    }


    private IEnumerator UnderscoreCoroutine()
    {
        bool hasPutUnderscore = false;
        
        while (true)
        {
            if(!hasPutUnderscore)
                textMesh.text += '?';
            else
                textMesh.text = textMesh.text.Remove(textMesh.text.Length - 1, 1);

            hasPutUnderscore = !hasPutUnderscore;

            yield return new WaitForSeconds(timeBetweenUnderscore);
        }
    }
}
