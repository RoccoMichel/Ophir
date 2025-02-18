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

        // bad solution for when barrel has a mesh being rendered
        // should rather calculate this
        barrel.LookAt(playerView.TransformDirection(transform.forward)*distance);
        if (Physics.Raycast(playerView.position, Vector3.forward, out RaycastHit hit, distance, layerMask))
        {
            if (debug)
                Debug.DrawRay(barrel.position, Vector3.forward, Color.yellow, 0.2f);

            if (hit.transform.gameObject.GetComponent<Entity>() != null)
                DoDamage(hit.transform.gameObject.GetComponent<Entity>());
        }

        // Effects & Sounds at barrel Transform
    }
}