using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTarget", story: "Check if [target] is null", category: "Conditions", id: "dcdfa7df7d04e54f988fffc9ce97c36f")]
public partial class CheckTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        return Target.Value != null;
    }
}
