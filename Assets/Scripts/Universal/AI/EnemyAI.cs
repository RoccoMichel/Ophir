using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Base AI Settings")]
    public bool active = true;
    public Vector3 destination;
    public bool dynamicStates = true;
    public bool eyesOnPlayer;
    public EnemyStates state;
    [Space(10)]
    public float minDistanceFromPlayer = 100f;
    public float wanderRadius = 8f; // actually acts like a width
    public float exploreRadius = 4f; // actually acts like a width

    protected Enemy self;
    protected NavMeshAgent agent;
    protected GameObject player;

    protected float distanceFromPlayer;
    protected Vector3 startPosition;
    protected Vector3 lastPointOfInterest;
    
    public enum EnemyStates
    {
        /// <summary>
        /// Plain dumb, if it is outside player view or dead
        /// </summary>
        None,

        /// <summary>
        /// Walking around the start position
        /// </summary>
        Wander,

        /// <summary>
        /// Explore around last point of interest
        /// </summary>
        Explore,

        /// <summary>
        /// Trying catch a target
        /// </summary>
        Pursue,

        /// <summary>
        /// Halting and attempting to attack a target
        /// </summary>
        Attack,

        /// <summary>
        /// Return to the start Position
        /// </summary>
        Return,

        /// <summary>
        /// Hide from the target
        /// </summary>
        Flee,
    }

    private void Start()
    {
        // Setting values
        startPosition = transform.position;
        lastPointOfInterest = startPosition;

        // Null checks on trivial values

        if (agent == null)
        {
            try
            {
                agent = GetComponent<NavMeshAgent>();
            }
            catch
            {
                if (GetComponentInChildren<NavMeshAgent>() != null) agent = GetComponentInChildren<NavMeshAgent>();
                else if (GetComponentInParent<NavMeshAgent>() != null) agent = GetComponentInParent<NavMeshAgent>();
                else
                {
                    Debug.LogError("EnemyAI is missing NavMeshAgent Component!");
                    gameObject.SetActive(false);
                }
            }
        }

        if (self == null)
        {
            try
            {
                self = GetComponent<Enemy>();
            }
            catch
            {
                if (GetComponentInChildren<Enemy>() != null) self = GetComponentInChildren<Enemy>();
                else if (GetComponentInParent<Enemy>() != null) self = GetComponentInParent<Enemy>();
                else Debug.LogError("EnemyAI is missing Entity Component!");
            }
        }

        if (player == null)
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            catch
            {
                player = FindAnyObjectByType<BasePlayer>().gameObject;
            }

            if (player == null) Debug.LogError("EnemyAI cannot find the Player");
        }
    }

    private void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer > minDistanceFromPlayer) active = false;
        else active = true;

        if (!active) return;

        ActOnState();
        agent.destination = destination;
    }

    public void LookForPlayer()
    {
        // create a ray between between player and enemy
        // if ray in cone vision of enemy front
        // and non-obstructed
        // set [bool] eyesOnPlayer
        // set state to EnemyStates.Pursue
    }

    public bool IsAtDestination()
    {
        if (!agent.pathPending && agent.pathStatus != NavMeshPathStatus.PathComplete)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude != 0f)
            return false;

        return true;
    }

    protected void ActOnState()
    {
        switch (state)
        {
            case EnemyStates.Wander:
                OnWander();
                break;

            case EnemyStates.Explore:
                OnExplore();
                break;

            case EnemyStates.Pursue:
                OnPursue();
                break;

            case EnemyStates.Attack:
                OnAttack();
                break;

            case EnemyStates.Return:
                OnReturn();
                break;

            case EnemyStates.Flee:
                OnFlee();
                break;
        }
    }

    /// <summary>
    /// Destination is set to a random position closely around the start position
    /// </summary>
    protected void OnWander()
    {
        // Still actively wandering
        if (!IsAtDestination()) return;

        // Create a new wander destination
        Vector3 newDestination = destination;

        while (wanderRadius / 2 > Vector3.Distance(newDestination, destination))
        {
            Vector3 randomPos = new (Random.Range(-wanderRadius,wanderRadius), 0, Random.Range(wanderRadius, -wanderRadius));
            newDestination = startPosition + randomPos;
        }

        destination = newDestination;
    }

    /// <summary>
    /// Explore last point of interest
    /// </summary>
    protected void OnExplore()
    {
        // Still actively wandering
        if (!IsAtDestination()) return;

        // Create a new wander destination
        Vector3 newDestination = destination;

        while (exploreRadius / 2 > Vector3.Distance(newDestination, destination))
        {
            Vector3 randomPos = new
                (Random.Range(-exploreRadius, exploreRadius), 0, Random.Range(exploreRadius, -exploreRadius));
            newDestination = lastPointOfInterest + randomPos;
        }

        destination = newDestination;
    }

    /// <summary>
    /// Chase the player
    /// </summary>
    protected void OnPursue()
    {
        if (eyesOnPlayer)
        {
            lastPointOfInterest = player.transform.position;
            destination = player.transform.position;
        }
        else
        {
            destination = lastPointOfInterest;
            if (IsAtDestination()) ChangeState(EnemyStates.Explore);
        }
    }

    /// <summary>
    /// Stand in place to Attack
    /// </summary>
    protected void OnAttack()
    {
        destination = transform.position;

        player.GetComponent<Entity>().TakeDamage(self.damage);
    }

    /// <summary>
    /// Return to Enemy's starting position
    /// </summary>
    protected void OnReturn()
    {
        destination = startPosition;
    }

    /// <summary>
    /// Hide from the player
    /// </summary>
    protected void OnFlee()
    {
        // I AM NOT PROGRAMMING THIS MAN!
    }

    public void ChangeState(EnemyStates newState)
    {
        if (!dynamicStates) return;

        state = newState;
    }
}