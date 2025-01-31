using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 7f;
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public float projectileSpeed = 8f;
    public GameObject bonePrefab;

    private Transform player;
    private Rigidbody2D rb2D;
    private Animator animator;
    private float lastAttackTime;
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If player is in range
        if (distanceToPlayer <= detectionRange)
        {
            // If in attack range, stop and shoot
            if (distanceToPlayer <= attackRange)
            {
                rb2D.velocity = Vector2.zero;
                animator.SetBool(IsMoving, false);
                TryAttack();
            }
            // Otherwise move towards player
            else
            {
                Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
                rb2D.velocity = direction * moveSpeed;
                animator.SetBool(IsMoving, true);

                // Flip sprite based on movement direction
                transform.localScale = new Vector3(
                    direction.x > 0 ? 1 : -1,
                    1,
                    1
                );
            }
        }
        // If player out of range, stop moving
        else
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool(IsMoving, false);
        }
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        SoundManager.Instance.PlayEnemySpell();

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

        GameObject bone = Instantiate(bonePrefab, transform.position, Quaternion.identity);
        Rigidbody2D boneRb = bone.GetComponent<Rigidbody2D>();

        if (boneRb != null)
        {
            boneRb.velocity = direction * projectileSpeed;

            // Rotate bone to face direction of travel
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        lastAttackTime = Time.time;
    }

    // Visualize ranges in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}