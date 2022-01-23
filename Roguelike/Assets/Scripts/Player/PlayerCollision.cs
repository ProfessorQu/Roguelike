using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerDash dash;

    public DashUI dashUI;

    private void Start() {
        dash = GetComponent<PlayerDash>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Enemy")) {
            if (!dash.isDashing) {
                if (dash.dashesLeft >= 1f) {
                    dash.dashesLeft =  Mathf.Floor(dash.dashesLeft - 1);
                    dashUI.Damage();
                }
                else {
                    Destroy(gameObject);
                }
            }
        }
    }
}
