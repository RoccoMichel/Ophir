using UnityEngine;

/// <summary>
/// Guns that shoot a physical projectile
/// </summary>
public class ProjectileGun : RangedWeapon
{
    [Header("Projectile Settings")]
    public GameObject projectile;
    public float force = 10f;

    public override void Shoot()
    {
        base.Shoot();
        
        Instantiate(projectile, barrel.position, barrel.rotation).TryGetComponent(out Rigidbody rigidbody);
        if (rigidbody != null) rigidbody.AddForce(barrel.forward * force, ForceMode.Impulse);
    }
}