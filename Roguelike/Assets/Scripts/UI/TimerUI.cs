using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    private TMP_Text text;

    private void Start() {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        float time = Game_Manager.Instance.time;
        
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time) - (minutes * 60);

        string minutesString = minutes.ToString("0##").Substring(1);
        string secondsString = seconds.ToString("0##").Substring(1);

        text.text = minutesString + ":" + secondsString;
    }
}
