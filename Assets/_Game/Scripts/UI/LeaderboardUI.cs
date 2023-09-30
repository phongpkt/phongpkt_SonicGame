using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    //1st Place
    [SerializeField] private TMP_Text timesText_1st;
    [SerializeField] private TMP_Text goldText_1st;
    //2nd Place
    [SerializeField] private TMP_Text timesText_2nd;
    [SerializeField] private TMP_Text goldText_2nd;
    //3rd Place
    [SerializeField] private TMP_Text timesText_3rd;
    [SerializeField] private TMP_Text goldText_3rd;

    public List<HighscoreEntryBase> highscoreEntryList = new List<HighscoreEntryBase>();
    private void OnEnable()
    {
        //Sort by gold
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                //Swap
                if (highscoreEntryList[j].gold >= highscoreEntryList[i].gold)
                {
                    HighscoreEntryBase tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }
        DisplayOnUI();
    }
    private void DisplayOnUI()
    {
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            string time = highscoreEntryList[i].currentTime.ToString();
            string gold = highscoreEntryList[i].gold.ToString();
            switch (i)
            {
                case 0: //1st
                    timesText_1st.SetText("Time: " + time);
                    goldText_1st.SetText("Gold: " + gold);
                    break;
                case 1: //2nd
                    timesText_2nd.SetText("Time: " + time);
                    goldText_2nd.SetText("Gold: " + gold);
                    break;
                case 2: //3rd
                    timesText_3rd.SetText("Time: " + time);
                    goldText_3rd.SetText("Gold: " + gold);
                    break;
            }
        }
    }
}
