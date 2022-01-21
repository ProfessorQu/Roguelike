using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;

    public float speed = 8f;
        public Vector2 direction;

    private bool facingRight = true;

    private bool freezeControl;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (!freezeControl) {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }
    }

    private void Update() {
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
        }
    }
}
