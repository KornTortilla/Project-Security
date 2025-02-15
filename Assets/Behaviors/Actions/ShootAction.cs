using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Shoot", story: "Shoot with [MouthGun]", category: "Action", id: "0bce3d99fe0eae929073e2f77ca0400f")]
public partial class ShootAction : Action
{
    [SerializeReference] public BlackboardVariable<MouthGun> MouthGun;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return MouthGun.Value.Shoot() ?  Status.Success : Status.Failure;
    }
}

