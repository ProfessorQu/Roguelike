using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public PlayerDash player;
    public Image charge;

    private Animator anim;

    [Space]
    public Color rechargingColor;
    public Color readyColor;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        // Update all dash images
        
        float fillAmount = Mathf.Clamp(player.dashLeft, 0, 1);

        // Set color
        if (fillAmount == 1f) {
            charge.color = readyColor;
        }
        else {
            charge.color = rechargingColor;
        }

        // Set fill amount
        charge.fillAmount = fillAmount;
    }

    public void Damage() {
        anim.SetTrigger("damage");
    }
}
