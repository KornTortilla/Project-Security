using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [CreateAssetMenu(fileName = "Player Attack Data", menuName = "ScriptableObjects/Player States/Attack State Data")]
    public class BasePlayerAttackData : ScriptableObject
    {
        public string animationName;
        public DamageInfo damageInfo;
    }
}