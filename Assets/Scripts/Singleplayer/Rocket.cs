using UnityEngine;

public class Rocket : Projectile
{
    [Header("Rocket Settings")]
    [Range(0f, 1f)]
    public float turnSpeed = 0.3f;
    public float moveSpeed = 50f;
    public float blastRadius = 2f;
    public float force = 10f;
    public bool homing;
    public Transform target;

    public override void OnStart()
    {
        base.OnStart();

        if (target == null && homing)
        {
            homing = false;
            Debug.LogWarning("Homing Rocket has no target! Homing disabled!");
        }
    }

    public override void Movement()
    {
        base.Movement();

        transform.position += moveSpeed * Time.deltaTime * transform.forward;

        if (homing)
        {
            Vector3 oldRotation = transform.rotation.eulerAngles;
            transform.LookAt(target);

            turnSpeed = Mathf.Clamp01(turnSpeed);
            transform.rotation = Quaternion.Euler(new Vector3
            {
                x = Mathf.Lerp(oldRotation.x, transform.rotation.eulerAngles.x, turnSpeed),
                y = Mathf.Lerp(oldRotation.y, transform.rotation.eulerAngles.y, turnSpeed),
                z = Mathf.Lerp(oldRotation.z, transform.rotation.eulerAngles.z, turnSpeed)
            });
        }
    }

    public override void Die()
    {
        Explode(damage, force, blastRadius);

        base.Die();
    }
}