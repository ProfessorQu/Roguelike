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
            anim.SetTrigger("show");
        }
        else {
            anim.SetTrigger("hide");
        }
    }

    private void ActivateAllChildren(bool activate) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(activate);
        }
    }
}
