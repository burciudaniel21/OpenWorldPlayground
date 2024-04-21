using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrol")]
    //Patrol
    public string playerTag = "Player"; // Tag of the player GameObject
    public float patrolRadius = 10f; // Radius within which the enemy patrols
    public float chaseRadius = 20f; // Radius within which the enemy starts chasing the player
    public float patrolSpeed = 3.5f; // Speed of patrolling
    public float chaseSpeed = 6f; // Speed of chasing
    public float patrolWaitTime = 2f; // Time the enemy waits at each patrol point

    private Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent;
    private Vector3 patrolPoint; // Random patrol point
    private bool isChasing = false;

    [Header("Attack")]
    //Attack
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackInterval = 1f;
    private HealthSystem playerHealth;
    private IEnumerator attackCoroutine; // Reference to the coroutine for attacking

    [Header("Enemy Health")]
    [SerializeField] private int maxHealth;

    public HealthSystem enemyHealthSystem;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomPatrolPoint();
        InvokeRepeating("CheckPlayerDistance", 0f, 0.5f); // Check player's distance periodically
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();

        attackCoroutine = AttackPlayerRepeatedly();
        StartCoroutine(attackCoroutine);

        enemyHealthSystem.maxHealth = maxHealth;
    }

    private void Update()
    {
        if (enemyHealthSystem.isDead == true)
        {
            //Stop the NavMeshAgent and the attack coroutine when the enemy dies
            if (enemyHealthSystem.isDead == true)
            {
                if(attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine); // Stop the attacking coroutine
                    navMeshAgent.isStopped = true;
                }
            }
        }
    }

    void CheckPlayerDistance()
    {
        if (Vector3.Distance(transform.position, player.position) <= chaseRadius)
        {
            StartChasing();
        }
        else if (isChasing)
        {
            StopChasing();
        }
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            StartCoroutine(Patrol()); // Start patrolling
        }
    }

    void StartChasing()
    {
        isChasing = true;
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position); // Chase the player
    }

    void StopChasing()
    {
        isChasing = false;
        navMeshAgent.speed = patrolSpeed;
        SetRandomPatrolPoint(); // Resume patrolling
    }

    IEnumerator Patrol()
    {
        // Wait at the patrol point for a while
        yield return new WaitForSeconds(patrolWaitTime);
        SetRandomPatrolPoint();
    }

    void SetRandomPatrolPoint()
    {
        // Generate a random point within the patrol radius
        Vector3 randomPoint = Random.insideUnitSphere * patrolRadius;
        randomPoint += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas);
        patrolPoint = hit.position;
        navMeshAgent.SetDestination(patrolPoint); // Set destination to the new patrol point
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private IEnumerator AttackPlayerRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            // Check if the player is within attack range
            if (Vector3.Distance(transform.position, playerHealth.transform.position) <= attackRange)
            {
                // Attack the player
                playerHealth.Damage(attackDamage);
            }
        }
    }
}