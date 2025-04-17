using UnityEngine;
using UnityEngine.AI;

public class AITarget : MonoBehaviour
{
    public GameObject target;
    public float attackRate = 1f;
    public float cooldown;

    Vector3 startPosition;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
    }
    private void Update()
    {
        if (target == null)
        {
            ReturnToStart();
            return;
        }

        agent.destination = target.transform.position;


        if (agent.remainingDistance <= 0.5f + agent.stoppingDistance)
        {
            cooldown = Mathf.Clamp(cooldown -= Time.deltaTime, 0, attackRate);
            if (cooldown == 0)
            {

                target.GetComponent<Entity>().TakeDamage(5);
                cooldown = attackRate;

                Debug.Log("attacked");
            }
        }
    }

    void ReturnToStart()
    {
        agent.destination = startPosition;
    }
}