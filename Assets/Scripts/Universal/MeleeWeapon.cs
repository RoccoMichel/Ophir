using UnityEngine;

/// <summary>
/// Weapons that do up close damage
/// </summary>
public class MeleeWeapon : Weapon
{
    /// <summary>
    /// The minimum time between attacks in Seconds
    /// </summary>
    [Header("Melee Setting")]
    public float cooldown;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (timeSinceLastShot >= cooldown && attackAction.WasPressedThisFrame())
            Attack();
    }

    public virtual void Attack()
    {
        timeSinceLastShot = 0;
        foreach (Entity entity in FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            if (Vector3.Distance(entity.transform.position, gameObject.transform.position) < distance)
                DoDamage(entity);
        }
    }
}