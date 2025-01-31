using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 3.0f;
    public float acceleration = 50f;
    public float deceleration = 50f;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float shootCooldown = 0.5f;

    private Vector2 movement = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;
    private Rigidbody2D rb2D;
    private Animator animator;
    private float lastShootTime;
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        // Handle shooting
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }

        // Update animation
        bool isMoving = movement.sqrMagnitude > 0.01f;
        animator.SetBool(IsMoving, isMoving);
    }

    void FixedUpdate()
    {
        // Calculate target velocity
        Vector2 targetVelocity = movement * movementSpeed;

        // Smoothly interpolate current velocity towards target velocity
        float accelRate = movement.sqrMagnitude > 0.01f ? acceleration : deceleration;
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            accelRate * Time.fixedDeltaTime
        );

        // Apply movement
        rb2D.velocity = currentVelocity;
    }

    void TryShoot()
    {
        if (Time.time - lastShootTime < shootCooldown)
            return;
        SoundManager.Instance.PlayPlayerSpell();

        // Get mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Create projectile
        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        // Set projectile velocity
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.velocity = shootDirection * projectileSpeed;

            // Optional: Rotate projectile to face direction of travel
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        lastShootTime = Time.time;
    }
}