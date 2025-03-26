using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [CreateAssetMenu(fileName = "StatActionAttributeData", menuName = "Scriptable Objects/Action Attribute Data/ Stat Attribute Data")]
    public class StatActionAttributeData : ActionAttributeData
    {
        public StatActionAttributeData()
        {
            attributeType = ActionAttributeType.StatChange;
        }

        public ActionStatChange[] actionStatChanges;
    }

    [System.Serializable]
    public struct ActionStatChange
    {
        public SpecialActionStat specialActionStat;
        public float arg;
    }
}
