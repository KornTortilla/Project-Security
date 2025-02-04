using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindTarget", story: "Agent checks if player is in [LineOfSight] and assigns it as [Target]", category: "Action", id: "e7a99590ebb5df3d61115a042a374667")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<LineOfSight> LineOfSight;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = LineOfSight.Value.Player;
        return LineOfSight.Value.PlayerIsInSight() ? Status.Success : Status.Failure;
    }

}

