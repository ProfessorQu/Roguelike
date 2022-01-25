using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    Rigidbody2D rb;

    private Player player;
    private Animator anim;

    [Header("Jump Variables")]
    public float jumpForce = 6f;

    public float jumpEndedMult = 0.5f;

    private bool isGrounded;

    [Header("Landing")]
    public GameObject dustParticles;

    [Header("Wall Jump Variables")]
    private Timer wallJumpTimer = new Timer();
    
    public Vector2 wallJumpForce;
    private float wallJumpDir;
    public float wallJumpTime;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        player = GetComponent<Player>();

        wallJumpTimer.SetTime(wallJumpTime);
    }

    private void Update() {
        anim.SetBool("isFalling", !player.IsGrounded());
    }

    private void FixedUpdate() {
        if (wallJumpTimer.Tick()) {
            wallJumpTimer.Reset();
            player.FreezeControl(false);

            rb.velocity = new Vector2(0, rb.velocity.y);
        } else if (wallJumpTimer.running) {
            rb.velocity = new Vector2(wallJumpForce.x * wallJumpDir, wallJumpForce.y);
        }
    }

    public void Land() {
        AudioManager.Instance.Play("Land");
        
        Collider2D ground = player.IsGrounded();
        if (ground) {
            player.SpawnDust(ground);
        }
    }
    
    public void Jump(InputAction.CallbackContext context) {
        if (context.started) {
            Collider2D wall = player.IsTouchingWall();

            if (player.IsGrounded()) {
                AudioManager.Instance.Play("Jump");

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                
                Collider2D ground = player.IsGrounded();
                player.SpawnDust(ground);

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
                player.FreezeControl(true);
            }
        }

        if (context.canceled && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpEndedMult);
        }
    }

}
