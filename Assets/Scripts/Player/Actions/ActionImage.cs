using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectSecurity.Gameplay
{
    public class ActionImage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        private RectTransform rectTransform;
        private Slider slider;

        private Vector2 targetPosition;
        private Coroutine moveCoroutine;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            slider = GetComponent<Slider>();
        }

        public void Initialize(string name)
        {
            textMesh.text = name;
        }

        public void SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = targetPosition = position;
        }

        public void UpdateFill(float progress)
        {
            slider.value = progress;
        }

        public void StartMove(Vector2 newPosition, float time, float setNewHeight = 0)
        {
            if (setNewHeight != 0)
            {
                // Debug.Log("New height: " + currentHeight);
                targetPosition = new Vector2(0f, setNewHeight);
            }

            rectTransform.anchoredPosition = targetPosition;

            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(Move(newPosition, time));
        }

        private IEnumerator Move(Vector2 newPosition, float time)
        {
            Vector2 startingPosition = rectTransform.anchoredPosition;

            targetPosition = newPosition;

            // Debug.Log(startingPosition);
            // Debug.Log(newPosition);

            float timer = 0f;
            while (timer < time)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, timer / time);

                timer += Time.deltaTime;

                yield return false;
            }

            rectTransform.anchoredPosition = targetPosition;
        }
    }
}