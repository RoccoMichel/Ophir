using UnityEngine;
/// <summary>
/// An Entity that will explode on Death & can burn before hand
/// </summary>
public class Explosive : Entity
{
    [Header("Explosive Settings")]
    public bool burnable = true;
    internal bool burning = false;
    public float damage = 20f;
    public float force = 10f;
    public float radius = 5f;
    public float burnAt = 50f;

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnStart()
    {
        if (burnAt >= health) Debug.LogWarning($"{gameObject.name} will start burning immediately!");
        if (!burnable && burnAt > 0) Debug.LogWarning($"{gameObject.name} will not burn as burnable is false!");
    }

    public virtual void OnUpdate()
    {
        
    }

    public override void Explode(float damage, float force, float radius)
    {
        base.Explode(damage, force, radius);
    }

    public override void Die()
    {
        Explode(damage, force, radius);

        base.Die();
    }
}
