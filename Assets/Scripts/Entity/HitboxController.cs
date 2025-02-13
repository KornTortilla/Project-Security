using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class HitboxController : MonoBehaviour
    {
        [SerializeField] private Transform hitboxStoringTransform;
        [SerializeField] private Hitbox defaultHitbox;

        private Dictionary<string, List<Hitbox>> hitboxDict;
        private string currentActionName;
        private HitboxData[] currentHitboxDatas;
        private Hitbox currentHitbox;
        private int hitboxDataIndex = -1;

        public void InitializeHitboxList(ActionData[] actionDatas)
        {
            hitboxDict = new Dictionary<string, List<Hitbox>>();

            foreach(ActionData actionData in actionDatas)
            {
                if (hitboxDict.ContainsKey(actionData.actionName)) continue;

                Transform actionStoringTransform = new GameObject().transform;
                actionStoringTransform.name = actionData.actionName;
                actionStoringTransform.parent = hitboxStoringTransform;

                List<Hitbox> hitboxList = new List<Hitbox>();
                foreach (GameObject hitboxPrefab in actionData.hitboxPrefabs)
                {
                    GameObject hitboxObject = Instantiate(hitboxPrefab, actionStoringTransform);
                    Hitbox hitbox = hitboxObject.GetComponent<Hitbox>();
                    hitboxObject.SetActive(false);

                    hitboxList.Add(hitbox);
                }

                hitboxDict.Add(actionData.actionName, hitboxList);
            }
        }

        public void SetCurrentAction(ActionData actionData)
        {
            currentActionName = actionData.actionName;
            currentHitboxDatas = actionData.hitboxDatas;

            hitboxDataIndex = -1;
        }

        public void NextHitbox()
        {
            if (currentHitbox != null)
                currentHitbox.gameObject.SetActive(false);

            hitboxDataIndex++;

            int hitboxIndex = currentHitboxDatas[hitboxDataIndex].hitboxIndex;
            currentHitbox = hitboxDict[currentActionName][hitboxIndex];

            currentHitbox.gameObject.SetActive(true);
            currentHitbox.Initialize(currentHitboxDatas[hitboxDataIndex].damageInfo);
        }

        public void ReuseLastHitbox()
        {
            currentHitbox.gameObject.SetActive(true);
        }

        public void InitalizeDefaultHitbox(DamageInfo damageInfo)
        {
            defaultHitbox.Initialize(damageInfo);
        }

        public void EnableDefaultHitbox()
        {
            currentHitbox = defaultHitbox;
            currentHitbox.gameObject.SetActive(true);
        }

        public void StopCurrentHitbox()
        {
            currentHitbox.gameObject.SetActive(false);
        }
    }
}