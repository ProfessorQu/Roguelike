using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("General")]
    public PlayerMove move;
    
    [Header("Jump")]
    public float rememberGroundedTime = 0.1f;
    private float groundedTimer;

    public float rememberJumpInputTime = 0.1f;
    private float rememberJumpTimer;

    [Space]
    public float jumpForce = 16f;
    public float jumpEndedFallSpeed = 0.5f;

    private bool jumpCanceled;
    
    // GROUND CHECK
    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public Vector2 groundCheckSize = new Vector2(2.6f, 1f);

    // WALL JUMP
    [Header("Wall Jump")]
    public Transform wallCheck;
    public float wallCheckSize = 0.1f;

    public Vector2 wallJumpForce = new Vector2(1f, 0.5f);

    public float wallJumpTime = 0.1f;
    private float wallJumpTimer;

    private Vector2 wallJumpVelocity;

    public float slidingSpeed = 3f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerMove>();
    }

    private void Update() {
        // Wall jump
        if (wallJumpTimer > 0f) {
           rb.velocity = wallJumpVelocity;
           
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0f) {
                move.FreezeControl(false);
            }
        }
        else if (IsTouchingWall() && rb.velocity.y < 0f) {
            move.fallSpeed = -slidingSpeed;
        }

        // Set grounded timer
        if (IsGrounded()) {
            groundedTimer = rememberGroundedTime;
        }
        
        // Jump
        if (groundedTimer > 0f && rememberJumpTimer > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            groundedTimer = 0f;
            rememberJumpTimer = 0f;
        }
        // Shorter jump
        else if (jumpCanceled) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedFallSpeed);
            jumpCanceled = false;
        }
        
        // Wall Jump
        Collider2D wall = IsTouchingWall();
        if (wall != null && rememberJumpTimer > 0f) {
            move.FreezeControl(true);
            float side = transform.position.x - wall.transform.position.x;
            if (side < 0) {
                side = -1f;
            }
            else if (side > 0) {
                side = 1f;
            }
            wallJumpVelocity = new Vector2(side * wallJumpForce.x, wallJumpForce.y);
            
            wallJumpTimer = wallJumpTime;
            rememberJumpTimer = 0f;
        }

        // Decrease timers
        groundedTimer -= Time.deltaTime;

        rememberJumpTimer -= Time.deltaTime;
    }
    
    private bool IsGrounded() {
        // Check if the player is grounded
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, whatIsGround);
    }
    
    private Collider2D IsTouchingWall() {
        // Check if the player is touching a wall
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckSize, whatIsGround);
    }

    public void Jump(InputAction.CallbackContext context) {
        // Get jump input
        if (context.performed) {
            rememberJumpTimer = rememberJumpInputTime;
        }

        if (context.canceled && rb.velocity.y > 0f) {
            jumpCanceled = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(wallCheck.position, wallCheckSize);
    }
}
