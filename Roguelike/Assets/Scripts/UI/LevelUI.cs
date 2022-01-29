using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    private TMP_Text text;

    private void Start() {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        text.text = "Level " + Game_Manager.Instance.currentLevel.ToString();
    }
}
