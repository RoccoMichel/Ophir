using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Settings")]
    public string identity = "Unnamed Entity";
    public float health = 100f;
    public bool isImmortal = false;
    public Substance substance;

    public enum Substance
    {
        /// <summary>
        /// Made of nothing or should display Nothing
        /// </summary>
        None,

        // Biological entities
        Flesh,     // Raw organic flesh or biological tissue
        Alien,     // Alien creatures, with different biological properties
        Plant,     // Plant-based entities or creatures (earth-based or alien flora)

        // Materials
        Glass,     // Glass-like entities or objects
        Concrete,  // Concrete-like entities or objects
        Wood,      // Wood-like entities or objects
        Metal,     // Metal-like entities or objects
        Stone,     // Stone-like entities or objects

        // Synthetic & Special Materials
        Synthetic, // Man-made or artificially created materials or creatures
        Crystal,   // Crystal-like entities or materials
        Liquid,    // Liquid-based entities or materials
        Energy,    // Pure energy-based entities or forms (e.g., plasma, magical energy)
    }

    public virtual void Explode(float damage, float force, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider victim in colliders)
        {
            Entity entity = victim.GetComponent<Entity>();
            Rigidbody rigidbody = victim.GetComponent<Rigidbody>();

            if (entity != null)
                entity.TakeDamage(damage);

            if (rigidbody != null)
                rigidbody.AddExplosionForce(force, transform.position, radius);
        }

        Die();
    }

    public virtual void TakeDamage(float damage)
    {
        // Play Effects & Sounds (load from resources)

        if (isImmortal) return;

        health -= damage;

        if (health <= 0) Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}