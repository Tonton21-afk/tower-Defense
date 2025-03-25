using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    public Player_Bullet gun;
    private Vector3 moveDirection;
    private Vector3 shootDirection;
    private bool recentlyShot;
    private Animator animator;

    private float shootCooldownTimer = 0f;
    public float shootCooldownDuration = 0.3f;

    // Dash Variables
    public float dashDistance = 5f;
    public float dashCooldown = 1f;
    private bool isDashing;
    private float dashCooldownTimer;
    private Vector3 dashTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (!rb) Debug.LogError("Rigidbody is missing on " + gameObject.name);
        if (!animator) Debug.LogError("Animator is missing on " + gameObject.name);

        rb.freezeRotation = true;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionY;

        animator.ResetTrigger("Dash");

        if (gun != null)
        {
            gun.OnShootDirection += RotatePlayerToShootDirection;
        }
    }

    void Update()
    {
        if (!LevelManager.instance.levelActive) return;

        if (!isDashing)
        {
            HandleMovementInput();
            HandleAnimations();
        }

        if (Input.GetButtonDown("Shoot"))
        {
            if (gun != null)
            {
                gun.Shoot();
                animator.SetTrigger("Shoot");

                recentlyShot = true;
                shootCooldownTimer = shootCooldownDuration;
            }
            else
            {
                Debug.LogError("Gun reference is missing in PlayerMovement!");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (recentlyShot && shootCooldownTimer > 0)
        {
            shootCooldownTimer -= Time.deltaTime;
        }
        else
        {
            recentlyShot = false;
        }
    }

    void RotatePlayerToShootDirection(Vector3 direction)
    {
        shootDirection = direction;
        transform.rotation = Quaternion.LookRotation(shootDirection);
    }

    void FixedUpdate()
    {
        if (!LevelManager.instance.levelActive) return;

        if (isDashing)
        {
            DashForward();
        }
        else
        {
            MovePlayer();
        }

        if (recentlyShot)
        {
            transform.rotation = Quaternion.LookRotation(shootDirection);
        }
        else
        {
            RotatePlayer();
        }
    }

    void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(-moveZ, 0, moveX).normalized;
    }

    void MovePlayer()
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }

    void RotatePlayer()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.deltaTime);
        }
    }

    void HandleAnimations()
    {
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isMoving);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Kapre_Shoot"))
        {
            animator.SetBool("isWalking", false);
        }
    }

    void StartDash()
    {
        animator.SetTrigger("isDashing");
        isDashing = true;

        Vector3 dashDirection = transform.forward;
        float intendedDashDistance = dashDistance;

        RaycastHit hit;
        bool obstacleDetected = Physics.Raycast(
            transform.position,
            dashDirection,
            out hit,
            dashDistance,
            LayerMask.GetMask("Tree")
        );

        if (obstacleDetected)
        {
            float buffer = 0.5f;
            intendedDashDistance = Mathf.Max(0, hit.distance - buffer);
        }

        dashTarget = transform.position + dashDirection * intendedDashDistance;
    }

    void DashForward()
    {
        rb.MovePosition(Vector3.MoveTowards(
            transform.position,
            dashTarget,
            moveSpeed * 10f * Time.fixedDeltaTime
        ));

        if (Vector3.Distance(transform.position, dashTarget) < 0.1f)
        {
            isDashing = false;
            dashCooldownTimer = dashCooldown;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            if (isDashing)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }

            Vector3 collisionNormal = collision.contacts[0].normal;
            Vector3 pushbackPosition = transform.position + collisionNormal * 0.2f;
            rb.MovePosition(pushbackPosition);

            rb.AddForce(collisionNormal * 2f, ForceMode.Impulse);
        }
    }
}
