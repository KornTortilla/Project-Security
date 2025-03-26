using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [CreateAssetMenu(fileName = "ActionAttributeData", menuName = "Scriptable Objects/Action Attribute Data/Base Attribute Data")]
    public class ActionAttributeData : ScriptableObject
    {
        public ActionAttributeType attributeType;
        public string description;
    }

    public enum ActionAttributeType
    {
        StatChange,
        CanSelfHarmCancel
    }
}
