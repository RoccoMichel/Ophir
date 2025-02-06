using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy Settings")]
    public float damage = 5f;
    public Transform target;

    public void Attack(Enemy target)
    {
        if (target == null) return;

        if (target.GetComponent<Entity>() == null) return;

        target.GetComponent<Entity>().TakeDamage(damage);

        try { FindAnyObjectByType<DamageIndicator>().InstantiateIndicator(transform, Color.red); }
        catch { }
    }
}
