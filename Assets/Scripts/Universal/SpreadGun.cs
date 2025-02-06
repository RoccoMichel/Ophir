using UnityEngine;

public class SpreadGun : RangedWeapon
{
    /// <summary>
    /// Number of pellets fired per shot
    /// </summary>
    [Header("Spread Settings")]
    public int pelletCount = 10;
    /// <summary>
    /// Spread angle in degrees
    /// </summary>
    public float spreadAngle = 15f;
    [Tooltip("Leave empty to use raycasts instead")]
    public GameObject pellet;
    public LayerMask layerMask;


    public override void Shoot()
    {
        base.Shoot();

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 spreadDirection = barrel.transform.forward + new Vector3(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0);

            Debug.Log(spreadDirection);

            if (pellet != null)
            {
                // Shot physical pellet
                Instantiate(pellet, barrel.position, barrel.rotation).TryGetComponent(out Rigidbody rigidbody);
                if (rigidbody != null) rigidbody.AddForce(spreadDirection * distance, ForceMode.Impulse);
            }
            else
            {
                // Raycast
                if (Physics.Raycast(barrel.position, barrel.TransformDirection(spreadDirection), out RaycastHit hit, distance, layerMask))
                {
                    if (debug)
                        Debug.DrawRay(barrel.position, barrel.TransformDirection(spreadDirection) * hit.distance, Color.yellow, 0.2f);

                    if (hit.transform.gameObject.GetComponent<Entity>() != null)
                        DoDamage(hit.transform.gameObject.GetComponent<Entity>());
                }
            }
        }

        // Effects & Sounds
    }
}