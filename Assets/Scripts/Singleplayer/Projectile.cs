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

    public virtual void Impact(Collision collision)
    {
        if (collision.transform.GetComponent<Entity>() != null)
            collision.transform.GetComponent<Entity>().TakeDamage(damage);

        if (dieOnImpact) Die();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Impact(collision);
    }
}