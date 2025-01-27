using UnityEngine;

public class Rocket : Projectile
{
    [Header("Rocket Settings")]
    public float speed = 50f;
    public float blastRadius = 2f;
    public float force = 10f;
    public bool homing = false;
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

        transform.position += speed * Time.deltaTime * transform.forward;

        if (homing)
        {
            transform.LookAt(target);
        }
    }

    public override void Die()
    {
        Explode(damage, force, blastRadius);

        base.Die();
    }
}