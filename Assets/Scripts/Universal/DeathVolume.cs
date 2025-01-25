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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() == null) return;

        if (!effectAll)
        {
            foreach(string tag in tags)
            {
                if (other.CompareTag(tag)) break;

                if (tag == tags[tags.Length - 1]) return;
            }
        }

        if (instantDeath) other.gameObject.GetComponent<Entity>().Die();
        else other.gameObject.GetComponent<Entity>().TakeDamage(damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Entity>() == null) return;

        if (!effectAll)
        {
            foreach (string tag in tags)
            {
                if (collision.gameObject.CompareTag(tag)) break;

                if (tag == tags[tags.Length - 1]) return;
            }
        }

        if (instantDeath) collision.gameObject.GetComponent<Entity>().Die();
        else collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
    }
}