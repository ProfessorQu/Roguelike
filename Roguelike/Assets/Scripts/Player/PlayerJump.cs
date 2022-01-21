using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Static Variables")]
    public PlayerMove move;
    private Animator anim;

    [Header("Gizmos")]
    public bool groundCheckGizmos;
    public bool wallCheckGizmos;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize;
    public LayerMask whatIsGround;

    [Header("Jump Variables")]
    public float jumpForce = 6f;

    public float jumpEndedMult = 0.5f;

    [Header("Wall Check")]
    public Transform wallCheck;
    public Vector2 wallCheckSize;

    [Header("Wall Jump Variables")]
    private Timer wallJumpTimer = new Timer();
    
    public Vector2 wallJumpForce;
    private float wallJumpDir;
    public float wallJumpTime;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        wallJumpTimer.SetTime(wallJumpTime);
    }

    private void Update() {
        if (IsGrounded()) {
            anim.SetBool("isJumping", false);
        }
        else {
            anim.SetBool("isJumping", true);
        }
    }

    private void FixedUpdate() {
        if (wallJumpTimer.Tick()) {
            wallJumpTimer.Reset();
            move.FreezeControl(false);

            rb.velocity = new Vector2(0, rb.velocity.y);
        } else if (wallJumpTimer.running) {
            rb.velocity = new Vector2(wallJumpForce.x * wallJumpDir, wallJumpForce.y);
        }
    }

    public Collider2D IsTouchingWall() {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, whatIsGround);
    }

    public bool IsGrounded() {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, whatIsGround);
    }
    
    public void Jump(InputAction.CallbackContext context) {
        if (context.started) {
            Collider2D wall = IsTouchingWall();

            if (IsGrounded()) {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                anim.SetTrigger("takeOff");
            }
            else if (wall != null) {
                wallJumpDir = transform.position.x - wall.transform.position.x;
                if (wallJumpDir < 0f) {
                    wallJumpDir = -1f;
                }
                else {
                    wallJumpDir = 1f;
                }

                wallJumpTimer.Start();
                move.FreezeControl(true);
            }
        }

        if (context.canceled && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedMult);
        }
    }

    private void OnDrawGizmos() {
        if (groundCheckGizmos) {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(groundCheck.position, groundCheckSize);
        }

        if (wallCheckGizmos) {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(wallCheck.position, wallCheckSize);
        }
    }
}
