using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TakeDamage", story: "[Health] decreased", category: "Conditions", id: "45456648e113a5033a33603bd24fbdfe")]
public partial class TakeDamageCondition : Condition
{
    [SerializeReference] public BlackboardVariable<EnemyHealth> Health;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
        
    }
}
