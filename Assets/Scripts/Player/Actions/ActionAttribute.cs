using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [CreateAssetMenu(fileName = "ActionAttributeData", menuName = "Scriptable Objects/Action Attribute Data")]
    public class ActionAttributeData : ScriptableObject
    {
        public SpecialActionStat specialActionStat;
        public float arg;
    }

    public enum SpecialActionStat
    {
        FlatDamange,
        PercentDamage,
        RechargeSpeed,
        MaxCharges,
        AnimationSpeed,
        CanSelfHarmCancel
    }
}
