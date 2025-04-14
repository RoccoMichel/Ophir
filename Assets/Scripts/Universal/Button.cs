using UnityEngine;

public class Button : Interactable
{
    [Header("Button Settings:")]
    public bool oneUse;

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (interactAction.WasPressedThisFrame())
        {
            Interaction();
        }
    }

    public override void Interaction()
    {
        if (oneUse) active = true;
        consequence.Invoke();
    }
}
