using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerDash dash;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Enemy")) {
            //if (!dash.isDashing) {
                //if (dash.dashesLeft >= 1f) {
                    //dash.dashesLeft--;
                //}
                //else {
                    //Destroy(gameObject);
                //}
            //}
            //else {
                Destroy(other.gameObject);
            //}
        }
    }
}
