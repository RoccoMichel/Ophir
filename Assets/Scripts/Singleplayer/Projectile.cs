using UnityEngine;

public class Projectile : Entity
{
    [Header("Projectile Settings")]
    public bool dieOnImpact;
    public float damage = 5f;

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

    }

    public virtual void OnUpdate()
    {
        Movement();
    }

    public virtual void Movement()
    {

    }

    public virtual void Impact()
    {
        if (dieOnImpact) Die();
    }
}
