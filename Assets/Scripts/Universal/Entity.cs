using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Settings")]
    public string identity = "Unnamed Entity";
    public float health = 100f;
    public bool isImmortal = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(1);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isImmortal) return;

        health -= damage;

        if (health <= 0) Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}