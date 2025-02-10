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

        if (Physics.Raycast(playerView.position, playerView.TransformDirection(Vector3.forward), out RaycastHit hit, distance, layerMask))
        {
            if (debug)
                Debug.DrawRay(playerView.position, playerView.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 0.2f);

            if (hit.transform.gameObject.GetComponent<Entity>() != null)
                DoDamage(hit.transform.gameObject.GetComponent<Entity>());
        }

        // Effects & Sounds at barrel Transform
    }
}