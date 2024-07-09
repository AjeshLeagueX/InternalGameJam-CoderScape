using System.Collections;
using UnityEngine;

public class EnemyDroneAI : MonoBehaviour
{
    public Transform player;
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

    private void Start()
    {
        player = PlayerController.Instance.gameObject.transform;
        ReloadBomb();
    }

    private void Update()
    {
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
    }

    private void RepositionDrone()
    {
        Vector3 targetPosition = player.position + player.forward * hoverDistance + Vector3.up * hoverHeight;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, repositionSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void AttackPlayer()
    {
        // Ensure drone doesn't reposition while attacking
        if (!alreadyAttacked)
        {
            // Attack code here
            ShootBomb();
            alreadyAttacked = true;

            if (attackWaitCoroutine != null)
                StopCoroutine(attackWaitCoroutine);
            attackWaitCoroutine = StartCoroutine(ResetAttack());
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
        Rigidbody rb = bugBomb.body;
        rb.transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 shootDirection = (player.position - bombSpawnT.position).normalized;
        rb.AddForce(shootDirection * 20f, ForceMode.Impulse); // Adjust the force as needed

        bugBomb.AutoDestroy();
        bugBomb = null;
        ReloadBomb();
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyDrone), 0.5f);
    }

    private void DestroyDrone()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
