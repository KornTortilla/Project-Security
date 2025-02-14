using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class HitboxController : MonoBehaviour
    {
        [SerializeField] private Transform hitboxStoringTransform;
        [SerializeField] private Hitbox defaultHitbox;

        private PlayerCharacterController playerCharacterController;

        private Dictionary<string, List<Hitbox>> hitboxDict;
        private string currentActionName;
        private HitboxData[] currentHitboxDatas;
        private Hitbox currentHitbox;
        private int hitboxDataIndex = -1;

        private void Awake()
        {
            playerCharacterController = GetComponent<PlayerCharacterController>();
        }

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
            DamageInfo damageInfo = currentHitboxDatas[hitboxDataIndex].damageInfo;
            currentHitbox.Initialize(damageInfo.damage, OrientKnockback(damageInfo.knockbackVector));
        }

        public void ReuseLastHitbox()
        {
            currentHitbox.gameObject.SetActive(true);
        }

        public void InitalizeDefaultHitbox(DamageInfo damageInfo)
        {
            defaultHitbox.Initialize(damageInfo.damage, OrientKnockback(damageInfo.knockbackVector));
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

        private Vector3 OrientKnockback(Vector3 knockbackVector)
        {
            return VectorUtility.OrientVectorHorizontal(knockbackVector, 
                playerCharacterController.CharacterForward, playerCharacterController.CharacterRight);
        }
    }
}