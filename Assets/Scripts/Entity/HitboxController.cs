using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    public class HitboxController : MonoBehaviour
    {
        [SerializeField] private Transform hitboxStoringTransform;
        [SerializeField] private Hitbox defaultHitbox;
        [SerializeField] private TrailRenderer defaultTrailRenderer;

        // Own event to signal
        public static event Action OnHit;

        private PlayerCharacterController playerCharacterController;
        private MeterManager meterManager;
        private EntityAudioController audioController;

        private Dictionary<string, List<Hitbox>> hitboxDict;
        private string currentActionName;
        private HitboxData[] currentHitboxDatas;
        private Hitbox currentHitbox;
        private int hitboxDataIndex = -1;

        private void Awake()
        {
            playerCharacterController = GetComponent<PlayerCharacterController>();
            meterManager = GetComponent<MeterManager>();
            audioController = GetComponent<EntityAudioController>();

            hitboxDict = new Dictionary<string, List<Hitbox>>();

            defaultHitbox.AddOnHitListener(() => Hit());
        }

        public void AddHitboxList(ActionData actionData)
        {
            if (hitboxDict.ContainsKey(actionData.actionName)) return;
            if (actionData.hitboxDatas.Length == 0) return;

            Transform actionStoringTransform = new GameObject().transform;
            actionStoringTransform.name = actionData.actionName;
            actionStoringTransform.parent = hitboxStoringTransform;

            List<Hitbox> hitboxList = new List<Hitbox>();
            foreach (GameObject hitboxPrefab in actionData.hitboxPrefabs)
            {
                GameObject hitboxObject = Instantiate(hitboxPrefab, transform.position, Quaternion.identity, actionStoringTransform);
                hitboxObject.transform.localPosition += hitboxPrefab.transform.localPosition;

                Hitbox hitbox = hitboxObject.GetComponent<Hitbox>();
                hitbox.AddOnHitListener(() => Hit());
                hitboxObject.SetActive(false);

                hitboxList.Add(hitbox);
            }

            hitboxDict.Add(actionData.actionName, hitboxList);
        }

        public void Hit()
        {
            TimeManager.main.Freeze(0.05f);

            audioController.PlayHit();
            meterManager.ChangeMeter(5);

            OnHit?.Invoke();
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

            defaultTrailRenderer.gameObject.SetActive(true);
        }

        public void EnableTrail()
        {
            defaultTrailRenderer.gameObject.SetActive(true);
        }

        public void StopCurrentHitbox()
        {
            if (currentHitbox == null) return;
            else if(defaultTrailRenderer.gameObject.activeSelf)
                defaultTrailRenderer.gameObject.SetActive(false);

            currentHitbox.gameObject.SetActive(false);
        }

        private Vector3 OrientKnockback(Vector3 knockbackVector)
        {
            return VectorUtility.OrientVectorHorizontal(knockbackVector, 
                playerCharacterController.lastLookDirection, playerCharacterController.CharacterRight);
        }
    }
}