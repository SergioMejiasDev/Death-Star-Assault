using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the timer in the scene.
/// </summary>
public class Timer : MonoBehaviour
{
    float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float TimerControl = Time.time - startTime;
        string mins = ((int)TimerControl / 60).ToString ("00");
        string secs = (TimerControl % 60).ToString ("00");
        string millisecs = ((TimerControl * 100) % 100).ToString("00");

        string TimerString = string.Format("{00}:{01}:{02}", mins, secs, millisecs);

        GetComponent<Text>().text = TimerString.ToString();
    }
}
