using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float chaseRange = 10f; // Range within which the enemy will chase the player
    public float patrolRange = 20f; // Range within which the enemy will randomly patrol
    public float patrolSpeed = 2f; // Speed of patrolling
    public float chaseSpeed = 5f; // Speed of chasing

    private NavMeshAgent navMeshAgent;
    private Vector3 patrolPoint;
    private bool isChasing = false;
    private Vector3 initialPosition;

    void Start()
    {
        navMeshAgent = gameObject.AddComponent<NavMeshAgent>(); // Add NavMeshAgent component
        initialPosition = transform.position;
        navMeshAgent.speed = patrolSpeed; // Set initial speed
        navMeshAgent.autoBraking = false; // Disable auto braking
        SetRandomPatrolDestination(); // Set initial patrol destination
        InvokeRepeating("UpdatePatrolDestination", 0f, 5f); // Change patrol destination every 5 seconds
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else if (isChasing)
        {
            StopChasing();
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        isChasing = true;
        navMeshAgent.speed = chaseSpeed;
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    void StopChasing()
    {
        isChasing = false;
        navMeshAgent.speed = patrolSpeed;
    }

    void Patrol()
    {
        if (!isChasing && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            SetRandomPatrolDestination();
        }
    }

    void SetRandomPatrolDestination()
    {
        patrolPoint = GetRandomPointInArea(initialPosition, patrolRange);
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(patrolPoint);
        }
    }

    Vector3 GetRandomPointInArea(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    void UpdatePatrolDestination()
    {
        if (!isChasing)
        {
            SetRandomPatrolDestination();
        }
    }
}
