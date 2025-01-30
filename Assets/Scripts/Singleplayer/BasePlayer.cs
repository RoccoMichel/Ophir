using UnityEngine;

public class BasePlayer : Entity
{
    [Header("Player Settings")]
    public float armor = 0;
    public float maxArmor = 100;
    public Weapon activeWeapon;


    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // Armor logic

        // Sound and Visual Indication
    }

    public virtual void AddArmor(int amount)
    {
        armor = Mathf.Clamp(armor + amount, 0, maxArmor);
    }

    public override void Die()
    {
        // Freeze player or something, anyway don't destroy it

        Debug.Log("Player Died");
    }
}