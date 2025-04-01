using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [CreateAssetMenu(fileName = "SpeicalActionData", menuName = "Scriptable Objects/Special Action Data")]
    public class ActionData : ScriptableObject
    {
        public string actionName;
        public float rechargeDuration;
        public string animationName;

        public GameObject[] hitboxPrefabs;
        public HitboxData[] hitboxDatas;

        public GameObject[] projectileObjects;

        [SerializeField] private SerializableStateType stateToEnter;

        public BaseState InstantiateNewState()
        {
            return PlayerStateCatalog.InstantiateState(stateToEnter.StateType);
        }
    }
}
