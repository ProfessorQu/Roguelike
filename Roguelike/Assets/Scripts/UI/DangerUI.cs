using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerUI : MonoBehaviour
{
    public PlayerDash dash;
    Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (dash.dashLeft < 1f) {
            anim.SetBool("danger", true);
        }
        else {
            anim.SetBool("danger", false);
        }
    }

    private void ActivateAllChildren(bool activate) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(activate);
        }
    }
}
