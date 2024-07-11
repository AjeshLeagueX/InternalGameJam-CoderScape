using MalbersAnimations.Utilities;
using System.Collections;
using UnityEngine;

public class EnemyDroneAI : MonoBehaviour
{
    public Transform bombSpawnT;
    private BugBomb bugBomb;

    public LayerMask whatIsPlayer;

    public float health;

    // Hovering
    public float hoverHeight = 5f;
    public float repositionSpeed = 3f;
    public float hoverDistance = 10f;

    // Attacking
    public float timeBetweenAttacks = 3f;
    private bool alreadyAttacked;
    private Coroutine attackWaitCoroutine;

    // Detection
    public float detectionRange = 20f, attackRange = 15f;
    private bool playerInSightRange, playerInAttackRange;
    Transform player => PlayerController.Instance.transform;

    // Separation
    public float separationDistance = 2f; // Minimum distance between enemies

    private void Start()
    {
        ReloadBomb();
    }

    private void Update()
    {
        if (player == null || EntitiesManager.Instance.IsEnemyFreeze)
            return;

        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, detectionRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange)
        {
            RepositionDrone();
            if (playerInAttackRange)
            {
                AttackPlayer();
            }
        }

        AvoidCollisionWithOtherEnemies();
    }

    private void RepositionDrone()
    {
        Vector3 targetPosition = player.position + player.forward * hoverDistance + Vector3.up * hoverHeight;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, repositionSpeed * Time.deltaTime);

        // Smoothly rotate to look at the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * repositionSpeed);
    }

    private void AvoidCollisionWithOtherEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.CompareTag("Enemy"))
            {
                Vector3 directionAway = transform.position - hitCollider.transform.position;
                transform.position += directionAway.normalized * (separationDistance - directionAway.magnitude) * Time.deltaTime;
            }
        }
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            ShootBomb();
        }
    }

    private void ReloadBomb()
    {
        bugBomb = ObjectSpawner.Instance.GetBugBomb();
        bugBomb.body.velocity = Vector3.zero;
        bugBomb.body.angularVelocity = Vector3.zero;

        bugBomb.transform.parent = bombSpawnT;
        bugBomb.transform.localPosition = Vector3.zero;
        bugBomb.transform.localRotation = Quaternion.identity;
    }

    private void ShootBomb()
    {
        Debug.Log("Shoot Bomb");
        if (bugBomb == null)
        {
            ReloadBomb();
            Debug.LogWarning("BugBomb is null. Cannot shoot.");
            return;
        }
        Debug.Log("Shoot Bomb 1");

        Rigidbody rb = bugBomb.body;
        if (rb == null)
        {
            Debug.LogError("Rigidbody is null. Cannot shoot.");
            return;
        }
        Debug.Log("Shoot Bomb 2");

        rb.transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;

        // Reset forces and clear all velocities to ensure it starts clean
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Apply force to shoot the bomb
        Vector3 shootDirection = (player.position - bombSpawnT.position).normalized;
        float force = 2f; // Adjust force as needed
        rb.AddForce(shootDirection * force, ForceMode.VelocityChange);

        Debug.Log("Bomb velocity: " + rb.velocity); // Log the velocity for debugging
        bugBomb.AutoDestroy();
        bugBomb = null;
        Debug.Log("Shoot Bomb 3");
        alreadyAttacked = true;
        ReloadBomb();

        if (attackWaitCoroutine != null)
            StopCoroutine(attackWaitCoroutine);
        attackWaitCoroutine = StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
    }
}
