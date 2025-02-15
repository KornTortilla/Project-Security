using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float Damage;
    [Min(0)]
    public float AttackSpeed;
}
