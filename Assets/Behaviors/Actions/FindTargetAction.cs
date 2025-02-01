using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Target", story: "[AgentTransform] finds first [tag] object within [range] units in [angle] degree FOV checking every [AngleBetweenRays] degrees apart as new [Target]", category: "Action", id: "cfaac083683546b79bd9e3388a64195a")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> AgentTransform;
    [SerializeReference] public BlackboardVariable<string> Tag;
    [SerializeReference][Min(0)] public BlackboardVariable<float> Range;
    [SerializeReference][Range(0,360f)] public BlackboardVariable<float> Angle;
    [SerializeReference][Min(1f)] public BlackboardVariable<float> AngleBetweenRays;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    private float halfAngle;
    Dictionary<string, List<GameObject>> objectsInSight = new();

    protected override Status OnStart()
    {
        halfAngle = Angle.Value / 2f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        CastLineOfSight();
        Target = objectsInSight.ContainsKey(Tag.Value) ? (BlackboardVariable<GameObject>)objectsInSight[Tag.Value][0] : null;
        return Target == null ? Status.Failure : Status.Success;
    }

    protected override void OnEnd()
    {
        objectsInSight.Clear();
    }

    private void CastLineOfSight() {
        Vector3 direction = AgentTransform.Value.forward;
        direction = Quaternion.AngleAxis(-halfAngle, AgentTransform.Value.up) * AgentTransform.Value.forward;
        for (float i = -halfAngle; i < halfAngle; i += AngleBetweenRays)
        {
            direction = Quaternion.AngleAxis(i, AgentTransform.Value.up) * AgentTransform.Value.forward;
            if(Physics.Raycast(AgentTransform.Value.position, direction, out RaycastHit hitInfo, Range)) {
                if (objectsInSight.TryAdd(hitInfo.collider.tag, new List<GameObject>())) {
                    objectsInSight[hitInfo.collider.tag].Add(hitInfo.collider.gameObject);
                } else if(!objectsInSight[hitInfo.collider.tag].Contains(hitInfo.collider.gameObject)) {
                    objectsInSight[hitInfo.collider.tag].Add(hitInfo.collider.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected() { 
        Vector3 direction = AgentTransform.Value.forward;
        direction = Quaternion.AngleAxis(-halfAngle, AgentTransform.Value.up) * AgentTransform.Value.forward;
        for (float i = -halfAngle; i < halfAngle; i += AngleBetweenRays)
        {
            Gizmos.color = Color.red;
            direction = Quaternion.AngleAxis(i, AgentTransform.Value.up) * AgentTransform.Value.forward;
            if(Physics.Raycast(AgentTransform.Value.position, direction, out RaycastHit hitInfo, Range)) {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(AgentTransform.Value.position, direction * hitInfo.distance);
            } else {
                Gizmos.DrawRay(AgentTransform.Value.position, direction * Range);
            }
        }
    }
}

