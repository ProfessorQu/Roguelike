using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{
    Rigidbody2D rb;

    private int direction = 1;
    private bool facingRight = true;

    public float speed = 10;

    [Header("Ground in front Check")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckSize = 0.1f;

    [Header("Front Check")]
    public Transform frontCheck;
    public Vector2 frontCheckSize = new Vector2(0.1f, 0.3f);

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (GetObjectsInFront() | !GroundInFront()) {
            direction *= -1;
        }
        
        // Flip character
        if (!facingRight && direction > 0f) {
            Flip();
        }
        else if (facingRight && direction < 0f) {
            Flip();
        }
    }

    private Collider2D GetObjectsInFront() {
        return Physics2D.OverlapBox(frontCheck.position, frontCheckSize, 0f);
    }

    private bool GroundInFront() {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckSize, whatIsGround);
    }

    private void Flip() {
        // Flip the player
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;

        transform.localScale = localScale;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(frontCheck.position, frontCheckSize);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(groundCheck.position, groundCheckSize);
    }
}
