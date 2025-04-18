using UnityEngine;

namespace ProjectSecurity.Gameplay
{
    [System.Serializable]
    public struct DamageInfo
    {
        public float damage;
        public Vector3 knockbackVector;
        public DamageType damageType;
    }

    public enum DamageType {light, heavy, DOT, AOE}
}