using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAI : MonoBehaviour
{
    Rigidbody2D rb;

    private int direction = 1;
    private bool facingRight = true;

    public float speed = 10;

    [Header("Wall Check")]
    public Transform wallCheck;
    public LayerMask whatIsWall;
    public float wallCheckSize = 0.1f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if (IsTouchingWall()) {
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

    private bool IsTouchingWall() {
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckSize, whatIsWall);
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
        Gizmos.DrawSphere(wallCheck.position, wallCheckSize);
    }
}
