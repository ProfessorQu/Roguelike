using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;

    PlayerJump jump;


    private Animator anim;

    public float speed = 8f;
    public Vector2 direction;

    private bool facingRight = true;

    private bool freezeControl;
    
    public GameObject dustParticles;
    public float timeBetweenDust = 1f;
    private Timer dustTimer = new Timer();

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        
        jump = GetComponent<PlayerJump>();

        anim = GetComponent<Animator>();

        dustTimer.SetTime(timeBetweenDust);
        dustTimer.Start();
    }

    private void FixedUpdate() {
        if (!freezeControl) {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        }
    }

    private void Update() {
        if (!freezeControl) {
            if (jump.IsGrounded() && !NotMoving() && dustTimer.Tick()) {
                Vector2 dustPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
                Instantiate(dustParticles, dustPos, Quaternion.identity);

                dustTimer.Reset();
                dustTimer.Start();
            }
        }

        if (facingRight && direction.x < 0f) {
            Flip();
        }
        else if (!facingRight && direction.x > 0f) {
            Flip();
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void FreezeControl(bool freeze) {
        freezeControl = freeze;
    }

    public void Move(InputAction.CallbackContext context) {
        if (!freezeControl) {
            direction = context.ReadValue<Vector2>().normalized;

            if (NotMoving()) {
                anim.SetBool("isRunning", false);
            }
            else {
                anim.SetBool("isRunning", true);
            }
        }
    }

    public bool NotMoving() {
        return Mathf.Abs(direction.x) < 0.1f && Mathf.Abs(direction.y) < 0.1f;
    }
}
