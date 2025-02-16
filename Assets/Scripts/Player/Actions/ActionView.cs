using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class ActionView : MonoBehaviour
    {
        [SerializeField] private float separationAmount;
        [SerializeField] private float timeToMove;
        [SerializeField] private GameObject actionImagePrefab;

        private List<ActionImage> actionImages;
        private int lastIndex = 0;

        public void Initialize(ActionData[] actionDatas)
        {
            actionImages = new List<ActionImage>();

            int maxHeight = actionDatas.Length - 1;
            for (int i = 0; i < actionDatas.Length; i++)
            {
                ActionImage newActionImage = Instantiate(actionImagePrefab, transform).GetComponent<ActionImage>();
                actionImages.Add(newActionImage);

                int currentHeight;
                if (i < actionDatas.Length - 1)
                    currentHeight = i;
                else
                    currentHeight = -1;

                newActionImage.Initialize(actionDatas[i].actionName, new Vector2(0f, separationAmount * currentHeight));
            }
        }

        public void Scroll(int newIndex, int direction)
        {
            int currentIndex = IntUtility.Wrap(lastIndex - 1, actionImages.Count - 1);
            for (int i = -1; i < actionImages.Count - 1; i++)
            {
                // Debug.Log("Current Index: " + currentIndex);
                ActionImage actionImage = actionImages[currentIndex];

                int newHeight = IntUtility.Wrap(i + direction, actionImages.Count - 2, -1);
                Vector2 newPosition = new Vector2(0f, separationAmount * newHeight);

                if (i == -1 && direction == -1)
                    actionImage.StartMove(newPosition, timeToMove, (actionImages.Count - 1) * separationAmount);
                else if(i == actionImages.Count - 2 && direction == 1)
                    actionImage.StartMove(newPosition, timeToMove, -2 * separationAmount);
                else
                    actionImage.StartMove(newPosition, timeToMove);

                currentIndex = IntUtility.Wrap(currentIndex + 1, actionImages.Count - 1);
            }

            lastIndex = newIndex;
        }

        public void UpdateImageProgress(int index, float progress)
        {
            actionImages[index].UpdateFill(progress);
        }
    }
}