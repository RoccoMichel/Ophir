using UnityEngine;

public class Door : Interactable
{
    [Header("Door Settings")]
    public bool open;
    public bool manualOpen;
    protected Animator doorAnimator;

    protected override void OnStart()
    {
        base.OnStart();

        if (!TryGetComponent(out doorAnimator))
            Debug.LogWarning("No Animator Component on Door Object");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        // Early return if not interactable in this frame
        if (!interactable || active) return;

        if (interactAction.WasPressedThisFrame() && manualOpen)
        {
            Interaction();
        }
    }

    public override void Interaction()
    {
        consequence.Invoke();
    }

    // Door Methods:

    public virtual void ToggleDoor()
    {
        if (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

        open = !open;
        DoorAnimation(open ? "Open" : "Close");
    }

    public virtual void SetDoor(bool status)
    {
        if (!doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

        open = status;
        DoorAnimation(open ? "Open" : "Close");
    }

    public virtual void DoorAnimation(string stateName)
    {
        doorAnimator.Play(stateName);
    }
}