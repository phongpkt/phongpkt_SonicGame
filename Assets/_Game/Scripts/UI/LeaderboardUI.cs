using Photon.Realtime;
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

    private List<HighscoreEntryBase> highscoreEntryList;
    private bool isData;

    private void Awake()
    {
        if(!isData)
        {
            highscoreEntryList = new List<HighscoreEntryBase>()
            {
                new HighscoreEntryBase { currentTime = "00:00", gold = 00},
                new HighscoreEntryBase { currentTime = "00:00", gold = 00},
                new HighscoreEntryBase { currentTime = "00:00", gold = 00},
            };

            HighScores highscore = new HighScores { highscoreEntryList = highscoreEntryList };
            string json = JsonUtility.ToJson(highscore);
            PlayerPrefs.SetString("HighscoreTable", json);
            PlayerPrefs.Save();
            isData = true;
        }
    }
    private void OnEnable()
    {
        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);

        //Sort by gold
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                //Swap
                if (highscores.highscoreEntryList[j].gold >= highscores.highscoreEntryList[i].gold)
                {
                    HighscoreEntryBase tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            string time = highscores.highscoreEntryList[i].currentTime.ToString();
            string gold = highscores.highscoreEntryList[i].gold.ToString();
            DisplayOnUI(time, gold, i);
        }
    }
    public void AddHighScoreEntry(string time, int coin)
    {
        HighscoreEntryBase highscoreEntry = new HighscoreEntryBase { currentTime = time, gold = coin };

        string jsonString = PlayerPrefs.GetString("HighscoreTable");
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);

        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("HighscoreTable", json);
        PlayerPrefs.Save();
    }
    private class HighScores 
    {
        public List<HighscoreEntryBase> highscoreEntryList;
    }

    private void DisplayOnUI(string time, string gold, int rank)
    {
        switch (rank)
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

    [System.Serializable]
    private class HighscoreEntryBase
    {
        public string currentTime;
        public int gold;
    }
}
