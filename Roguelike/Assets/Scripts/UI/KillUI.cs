using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillUI : MonoBehaviour
{
    private TMP_Text text;

    private void Start() {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        text.text = Game_Manager.Instance.kills.ToString();
    }
}
