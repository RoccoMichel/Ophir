using UnityEngine;

public class Destructible : Entity
{
    [Header("Destructible Settings")]
    [SerializeField] GameObject[] InstantiateOnDeath;

    public override void Die()
    {
        foreach (var obj in InstantiateOnDeath)
            Instantiate(obj, transform.position, Quaternion.identity);
        base.Die();
    }
}
