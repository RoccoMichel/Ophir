using UnityEngine;
using UnityEngine.Events;
public class Valve : Interactable
{
    [Header("Valve Settings:")]
    public UnityEvent<float> dependents;
    public bool oneUse;
    [Tooltip("Lose Progress when not actively turning")]
    public bool retract;
    public float crankSpeed = 1f;
    private float progress; // between 0 and 1


    protected override void OnUpdate()
    {
        dependents.Invoke(progress);
        base.OnUpdate();

        if (interactAction.IsPressed()) progress += crankSpeed * Time.deltaTime;
        else if (retract && progress > 0) progress = Mathf.Clamp01(progress -= crankSpeed * Time.deltaTime);

        if (progress >= 1) Interaction();
    }

    public override void Interaction()
    {
        consequence.Invoke();
        if (oneUse) active = true;

        // sound / effect / animation?
    }
}
