using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerExit : MonoBehaviour
{
    private bool canExit = false;

    public void ExitLevel(InputAction.CallbackContext context) {
        if (canExit) {
            bool success = Game_Manager.Instance.LoadNextLevel();
            if (success) {
                canExit = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Exit")) {
            canExit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Exit")) {
            canExit = false;
        }
    }
}
