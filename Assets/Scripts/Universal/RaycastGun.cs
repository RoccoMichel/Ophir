using UnityEngine;

/// <summary>
/// A firearm that shoots Raycast to calculate hits
/// </summary>
public class RaycastGun : RangedWeapon
{
    [Header("Raycast Settings")]
    public LayerMask layerMask;
    public override void Shoot()
    {
        base.Shoot();

        if (timeSinceLastShot < 60 / rmp || activeAmmo <= 0) return;

        if (Physics.Raycast(barrel.position, barrel.TransformDirection(Vector3.forward), out RaycastHit hit, distance, layerMask))
        {
            if (debug)
                Debug.DrawRay(barrel.position, barrel.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 0.2f);

            if (hit.transform.gameObject.GetComponent<Entity>() != null)
                DoDamage(hit.transform.gameObject.GetComponent<Entity>());
        }
    }
}