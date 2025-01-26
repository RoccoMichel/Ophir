using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    [Tooltip("fix any variable conflicts on Start")]
    [SerializeField] bool selfFix = true;
    [Header("Death Volume Properties")]
    [SerializeField] bool instantDeath = false;
    [SerializeField] bool effectAll = true;
    [SerializeField] string[] tags;
    [SerializeField] float damage = 10f;

    private void Start()
    {
        if (!selfFix) return;

        if (gameObject.GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing the Collider component! A temporary one has been added!");
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
        }

        if (!effectAll && tags.Length == 0)
        {
            Debug.LogWarning($"{gameObject.name} with effectAll turned off and no tags, Death Volume is useless. effectAll has been set to true!");
            effectAll = true;
        }

        if (effectAll && tags.Length > 0)
        {
            Debug.LogWarning($"{gameObject.name} tags assignments are unnecessary with effectAll active. effectAll has been set to false!");
            effectAll = false;
        }
    }

    // ------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        TriggerDamage(other);
    }
    private void OnTriggerStay(Collider other)
    {
        TriggerDamage(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionDamage(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        CollisionDamage(collision);
    }

    // ------------------------------------

    void CollisionDamage(Collision c)
    {
        if (c.gameObject.GetComponent<Entity>() == null) return;

        if (!effectAll)
        {
            foreach (string tag in tags)
            {
                if (c.gameObject.CompareTag(tag)) break;

                if (tag == tags[tags.Length - 1]) return;
            }
        }

        if (instantDeath) c.gameObject.GetComponent<Entity>().Die();
        else c.gameObject.GetComponent<Entity>().TakeDamage(damage * Time.deltaTime);
    }

    void TriggerDamage(Collider c)
    {
        if (c.gameObject.GetComponent<Entity>() == null) return;

        if (!effectAll)
        {
            foreach (string tag in tags)
            {
                if (c.CompareTag(tag)) break;

                if (tag == tags[tags.Length - 1]) return;
            }
        }

        if (instantDeath) c.gameObject.GetComponent<Entity>().Die();
        else c.gameObject.GetComponent<Entity>().TakeDamage(damage * Time.deltaTime);
    }
}