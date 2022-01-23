using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;

    private CinemachineVirtualCamera cam;
    private CinemachineFramingTransposer framingTransposer;

    private Animator anim;

    public float speed = 8f;
    public Vector2 direction;

    private bool facingRight = true;

    
    public float timeBetweenDust = 1f;
    private Timer dustTimer = new Timer();

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        cam = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        framingTransposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();

        anim = GetComponent<Animator>();

        dustTimer.SetTime(timeBetweenDust);
        dustTimer.Start();
    }

    private void FixedUpdate() {
        if (!player.freezeControl) {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }
    }

    private void Update() {
        if (!player.freezeControl) {
            Collider2D ground = player.IsGrounded();

            if (ground && !player.NotMoving()) {
                if (dustTimer.Tick()) {
                    player.SpawnDust(ground);

                    dustTimer.Reset();
                    dustTimer.Start();
                }
            }

            if (direction.y < 0f) {
                framingTransposer.m_ScreenY = 0f;
            }
            else {
                framingTransposer.m_ScreenY = 0.5f;
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

    

    public void Move(InputAction.CallbackContext context) {
        if (!player.freezeControl) {
            direction = context.ReadValue<Vector2>().normalized;

            if (player.NotMoving()) {
                anim.SetBool("isRunning", false);
            }
            else {
                anim.SetBool("isRunning", true);
            }
        }
    }
}
