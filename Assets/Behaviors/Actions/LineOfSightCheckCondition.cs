using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Line Of Sight Check", story: "Check if [target] is not null", category: "Conditions", id: "52392e20380597280c5ecfb522060c20")]
public partial class LineOfSightCheckCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        return Target != null;
    }
}
