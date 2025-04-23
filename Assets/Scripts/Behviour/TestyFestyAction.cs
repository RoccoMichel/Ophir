using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TestyFesty", story: "[hungy] [lungy] [bungy]", category: "Action", id: "fd1bce91fa071668e45df32a5ac04751")]
public partial class TestyFestyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Hungy;
    [SerializeReference] public BlackboardVariable<float> Lungy;
    [SerializeReference] public BlackboardVariable<Camera> Bungy;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

