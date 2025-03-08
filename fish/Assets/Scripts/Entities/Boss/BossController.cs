using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    private MeshRenderer meshRenderer;

    public float health;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Charging Attack
    public float chargeSpeed;
    public float chargeDuration;
    private bool isCharging = false;
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // Trap
    public bool hasJumped;
    public float jumpForce;
    public float jumpDuration;
    public float gravityMultiplier;

    // States
    public float sightRange, attackRange, trapRange;
    public bool playerInSightRange, playerInAttackRange, playerInTrapRange;

    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.Find("Boat").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.enabled = false;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInTrapRange = Physics.CheckSphere(transform.position, trapRange, whatIsPlayer);

        if (playerInTrapRange && !hasJumped) StartCoroutine(TriggerTrap());
        else if (!playerInSightRange && !playerInAttackRange && !playerInTrapRange && hasJumped) Patrolling();
        else if (playerInSightRange && !playerInAttackRange && hasJumped) ChasePlayer();
        else if (playerInAttackRange && hasJumped) AttackPlayer();
    }

    private IEnumerator TriggerTrap()
    {
        hasJumped = true;
        rb.isKinematic = false;
        meshRenderer.enabled = true;

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            rb.linearVelocity += Vector3.down * gravityMultiplier * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => Physics.Raycast(transform.position, Vector3.down, 1f, whatIsGround));

        rb.isKinematic = true;
        agent.enabled = true;

        if (playerInSightRange) ChasePlayer();
        else Patrolling();
    }


    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (!isCharging && hasJumped && agent.enabled)
            agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked && hasJumped)
        {
            alreadyAttacked = true;
            StartCoroutine(ChargeAtPlayer());
            Invoke(nameof(ResetAttack), timeBetweenAttacks + chargeDuration);
        }
    }


    private IEnumerator ChargeAtPlayer()
    {
        isCharging = true;
        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;  

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        float chargeTime = 0f;
        while (chargeTime < chargeDuration)
        {
            rb.MovePosition(rb.position + direction * chargeSpeed * Time.deltaTime);
            chargeTime += Time.deltaTime;
            yield return null;
        }

        rb.isKinematic = true;
        agent.enabled = true;
        isCharging = false;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, trapRange);
    }
}
