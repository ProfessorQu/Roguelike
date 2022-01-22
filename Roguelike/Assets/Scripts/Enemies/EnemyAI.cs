using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected bool facingRight = true;
    protected int direction = 1;
    
    protected void Flip() {
        // Flip the player
        facingRight = !facingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1;

        transform.localScale = localScale;
    }

    protected void UpdateFlip() {
        if (!facingRight && direction > 0f) {
            Flip();
        }
        else if (facingRight && direction < 0f) {
            Flip();
        }
    }

    public virtual void Kill() { }
}
