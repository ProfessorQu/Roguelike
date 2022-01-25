using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerExit : MonoBehaviour
{
    private bool canExit = false;

    public void ExitLevel(InputAction.CallbackContext context) {
        if (canExit) {
            Debug.Log("Exited level");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        canExit = other.CompareTag("Exit");
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Exit")) {
            canExit = false;
        }
    }
}
