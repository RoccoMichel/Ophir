using UnityEngine;

public class Grenade : Projectile
{
    [Header("Grenade Settings")]
    public float radius = 5f;
    public float force = 10f;
    // something for damage fall of by distance

    public override void OnUpdate()
    {
        base.OnUpdate();

        TakeDamage(Time.deltaTime);
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
