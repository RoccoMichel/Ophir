using UnityEngine;
using UnityEngine.Events;
public class Valve : Interactable
{
    [Header("Valve Settings:")]
    public UnityEvent<float> dependents;
    [Tooltip("Lose Progress when not actively turning")]
    public bool retract;
    public float crankSpeed = 1f;
    private float progress; // between 0 and 1


    protected override void OnUpdate()
    {
        base.OnUpdate();

        dependents.Invoke(progress); // float does not get taken as argument, rather inspector value
        
        // Early return if not interactable by player in this that frame
        if (!interactable || active) return;

        // Increase progress while interacting
        if (interactAction.IsPressed()) progress += crankSpeed * Time.deltaTime;
        else if (retract && progress > 0) progress -= crankSpeed * Time.deltaTime; // lose progress if enabled
        progress = Mathf.Clamp01(progress);

        // Interaction if done cranking
        if (progress >= 1) Interaction();
    }

    public override void RaycastInteraction()
    {
        // idk how this is going to work gang
    }

    public override void Interaction()
    {
        consequence.Invoke();
        active = true;

        // sound / effect / animation?
    }
}
