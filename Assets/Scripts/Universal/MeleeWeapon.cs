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


    public virtual void Attack()
    {
        foreach (Entity entity in FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            if (Vector3.Distance(entity.transform.position, gameObject.transform.position) < distance)
                DoDamage(entity);
        }
    }
}
