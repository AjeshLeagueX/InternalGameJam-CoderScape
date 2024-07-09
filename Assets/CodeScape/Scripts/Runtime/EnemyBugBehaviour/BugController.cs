using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BugController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 20f;
    public float biteRange = 2f;
    public float throwRange = 6f;
    public float moveSpeed = 3.5f;
    public int biteDamage = 20;
    public float attackCooldown = 1.5f;
    public GameObject codeSnippetPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;

    private bool isAttacking = false;
    private float attackCoolDownTime = 2f;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            FollowPlayer(distanceToPlayer);

            if (!isAttacking)
            {
                if (distanceToPlayer <= biteRange)
                {
                    //BitePlayer();
                }
                else if (distanceToPlayer <= throwRange)
                {
                    ThrowSnippet();
                }
            }
        }
        else
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("isWalking", false);
        }
    }

    void FollowPlayer(float distanceToPlayer)
    {
        if (!isAttacking)
        {
            navMeshAgent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
    }

    //void BitePlayer()
    //{
    //    // Ensure player is still in range
    //    if (Vector3.Distance(transform.position, player.position) <= biteRange)
    //    {
    //        isAttacking = true;
    //        player.GetComponent<PlayerController>().ApplyDamage(biteDamage);
    //        StartCoroutine(AttackCoolDown());
    //    }
    //}

    void ThrowSnippet()
    {
        isAttacking = true;

        var _snippet = ObjectSpawner.Instance.GetBugBomb();
        _snippet.transform.position = transform.position;
        Rigidbody snippetRb = _snippet.GetComponent<Rigidbody>();
        Vector3 direction = (player.position - throwPoint.position).normalized;
        snippetRb.AddForce(direction * throwForce, ForceMode.Impulse);

        StartCoroutine(AttackCoolDown());
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCoolDownTime);
        isAttacking = false;
    }

    void ResetAttack()
    {
        animator.SetBool("isWalking", true);
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection, bite, and throw ranges for debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, biteRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, throwRange);
    }
}
