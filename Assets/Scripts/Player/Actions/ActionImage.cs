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

        private int currentHeight;
        private int maxHeight;
        private float separationAmount;
        private Coroutine moveCoroutine;
        private bool hasMovedThisFrame;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            slider = GetComponent<Slider>();
        }

        public void Initialize(string name, int height, int maxHeight, float separationAmount)
        {
            textMesh.text = name;

            currentHeight = height;
            this.maxHeight = maxHeight;
            this.separationAmount = separationAmount;

            rectTransform.anchoredPosition = new Vector2(0, separationAmount * height);
        }

        public void UpdateFill(float progress)
        {
            slider.value = progress;
        }

        public void StartMove(int direction, float time)
        {
            if (hasMovedThisFrame) return;

            int projection = currentHeight + direction;
            if (projection == maxHeight) currentHeight = -2;
            else if (projection == -2) currentHeight = maxHeight;

            rectTransform.anchoredPosition = GetCurrentHeightPosition();

            currentHeight += direction;

            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(Move(time));

            hasMovedThisFrame = true;
            StartCoroutine(DelayMoveUpdate());
        }

        private IEnumerator Move(float time)
        {
            Vector2 startingPosition = rectTransform.anchoredPosition;
            Vector2 targetPosition = GetCurrentHeightPosition();

            float timer = 0f;
            while (timer < time)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, timer / time);

                timer += Time.deltaTime;

                yield return false;
            }

            rectTransform.anchoredPosition = targetPosition;
        }

        private IEnumerator DelayMoveUpdate()
        {
            yield return false;

            hasMovedThisFrame = false;
        }

        private Vector2 GetCurrentHeightPosition()
        {
            return new Vector2(0, separationAmount * currentHeight);
        }
    }
}