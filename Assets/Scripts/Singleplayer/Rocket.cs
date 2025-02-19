using UnityEngine;

public class Rocket : Projectile
{
    [Header("Rocket Settings")]
    [Range(0f, 1f)]
    public float turnSpeed = 0.3f;
    public float moveSpeed = 50f;
    public float radius = 2f;
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
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    public override void Die()
    {
        Explode(damage, force, radius);

        base.Die();
    }
}