using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    private float time;

    private TMP_Text text;

    private void Start() {
        text = GetComponentInChildren<TMP_Text>();
    }

    private void Update() {
        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time) - (minutes * 60);

        string minutesString = minutes.ToString("0##").Substring(1);
        string secondsString = seconds.ToString("0##").Substring(1);

        text.text = minutesString + ":" + secondsString;
    }
}
