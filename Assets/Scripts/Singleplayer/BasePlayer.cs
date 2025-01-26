using UnityEngine;

public class BasePlayer : Entity
{
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // Sound and Visual Indication
    }

    public override void Die()
    {
        // Freeze player or something, anyway don't destroy it

        Debug.Log("Player Died");
    }
}