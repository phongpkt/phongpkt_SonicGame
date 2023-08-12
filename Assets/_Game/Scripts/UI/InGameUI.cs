using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text timesText;

    private string currentTime;

    private float secondsCount;
    private int minuteCount;

    private void Update()
    {
        UpdateTimerUI();
        coinsText.SetText(player.coin.ToString());
        livesText.SetText(player.lives.ToString());
        timesText.SetText(currentTime);
    }
    public void UpdateTimerUI()
    {
        //set timer UI
        secondsCount += Time.deltaTime;
        currentTime = minuteCount + ":" + (int)secondsCount;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            minuteCount = 0;
        }
    }
}
