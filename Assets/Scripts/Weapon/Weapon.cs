using UnityEngine;
using ProjectSecurity.Gameplay;

public abstract class Weapon : MonoBehaviour
{
    public DamageInfo DamageInfo;
    [Min(0)]
    public float AttackSpeed;
}
