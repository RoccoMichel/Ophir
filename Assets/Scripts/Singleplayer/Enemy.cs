using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy Settings")]
    public float damage = 5f;

    public virtual void Attack(Enemy target)
    {
        if (target == null) return;

        if (target.GetComponent<Entity>() == null) return;

        target.GetComponent<Entity>().TakeDamage(damage);

        //// Expensive Search?
        //try { FindAnyObjectByType<DamageIndicator>().InstantiateIndicator(transform, Color.red); }
        //catch { }
    }
}
