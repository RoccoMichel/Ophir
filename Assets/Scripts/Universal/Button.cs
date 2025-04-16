using UnityEngine;

public class Button : Interactable
{
    [Header("Button Settings:")]
    public bool oneUse;

    protected override void OnUpdate()
    {
        base.OnUpdate();

        // Early return if not interactable in this frame
        if (!interactable || active) return;

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
