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

        public void Initialize(ActionData[] actionDatas)
        {
            actionImages = new List<ActionImage>();

            int maxHeight = actionDatas.Length - 1;
            for (int i = 0; i < actionDatas.Length; i++)
            {
                ActionImage newActionImage = Instantiate(actionImagePrefab, transform).GetComponent<ActionImage>();
                actionImages.Add(newActionImage);

                if (i < actionDatas.Length - 1) newActionImage.Initialize(actionDatas[i].actionName, i, maxHeight, separationAmount);
                else newActionImage.Initialize(actionDatas[i].actionName, -1, maxHeight, separationAmount);
            }
        }

        public void Scroll(int direction)
        {
            //actionImages[currentOutlier].SetPosition(-direction * separationAmount * 2f);

            foreach (ActionImage actionImage in actionImages)
            {
                actionImage.StartMove(direction, timeToMove);
            }

            //currentOutlier = IntUtility.Wrap(currentOutlier + (int)direction, 3);
        }

        public void UpdateImageProgress(int index, float progress)
        {
            actionImages[index].UpdateFill(progress);
        }
    }
}