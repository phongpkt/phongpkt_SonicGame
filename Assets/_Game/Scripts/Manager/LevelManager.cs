using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //Level
    public List<Map> levelMap = new List<Map>();
    [SerializeField] private int levelIndex;
    private Vector3 level1_StartingPosition = new Vector3(-48.5f, 0, 0);
    private Vector3 level2_StartingPosition = new Vector3(-45, 2.5f, 0);
    private Vector3 level3_StartingPosition = new Vector3(115, 50, 0);
    private GameObject map;
    private Vector3 mapPos = new Vector3(0, 0, 0);

    //Camera
    private GameObject mainCam;
    private GameObject finalScenceCam;
    public CinemachineVirtualCamera vCam;
    public CinemachineTargetGroup targetGroup;

    //Player + Boss
    public GameObject bossPrefab;
    public GameObject[] playerPrefabs;
    private GameObject boss;
    [SerializeField] private GameObject player;
    private Transform bossTF;
    private Transform playerTF;
    private int characterIndex;
    private Vector3 bossPos = new Vector3(130f, 50f, 0);

    //UI
    private UIManager UIManager;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private BossFightUI bossScenceUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    private void Awake()
    {
        OnInit();
    }
    private void OnInit()
    {
        levelIndex = 0;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        finalScenceCam = GameObject.FindGameObjectWithTag("FinalScenceCamera");
        UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        finalScenceCam.SetActive(false);
    }
    private void Update()
    {
        OnFinishLevel();
        OnLose();
        OnWin();
    }
    public void OnPlay()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter");
        ActiveLevel(levelIndex);
        player = Instantiate(playerPrefabs[characterIndex], level1_StartingPosition, Quaternion.identity);
        playerTF = player.transform;
        vCam.m_Follow = player.transform;
        inGameUI.player = player.GetComponent<Player>();
        SoundManager.Instance.PlayIngameSound();
    }
    public void Replay()
    {
        levelIndex = 0;
        finalScenceCam.SetActive(false);
        mainCam.SetActive(true);
        OnPlay();
    }
    public void ChangeStateToMainMenu()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
    }
    private void OnLose()
    {
        if(GameManager.Instance.IsState(GameState.GameOver))
        {
            SoundManager.Instance.StopIngameSound();
            SoundManager.Instance.StopBossFightSound();
            SoundManager.Instance.PlayWhenLose();

            Destroy(player);
            DeactiveLevel();
            playerTF = null;
            vCam.m_Follow = null;
            inGameUI.player = null;
            UIManager.ChangeUI(loseUI);

            //Save score
            int coins = PlayerPrefs.GetInt("coin");
            HighscoreEntryBase highscoreEntry = new HighscoreEntryBase(inGameUI.currentTime, coins);
            leaderboardUI.highscoreEntryList.Add(highscoreEntry);
            GameManager.Instance.ChangeState(GameState.GamePause);
        }
    }
    private void OnWin()
    {
        if (GameManager.Instance.IsState(GameState.GameWin))
        {
            SoundManager.Instance.StopIngameSound();
            SoundManager.Instance.StopBossFightSound();
            SoundManager.Instance.PlayWhenWin();

            Destroy(player);
            DeactiveLevel();
            playerTF = null;
            vCam.m_Follow = null;
            inGameUI.player = null;
            UIManager.ChangeUI(winUI);

            //Save score
            int coins = PlayerPrefs.GetInt("coin");
            HighscoreEntryBase highscoreEntry = new HighscoreEntryBase(inGameUI.currentTime, coins);
            leaderboardUI.highscoreEntryList.Add(highscoreEntry);
            GameManager.Instance.ChangeState(GameState.GamePause);
        }
    }
    private void ActiveLevel(int level)
    {
        map = Instantiate(levelMap[level].gameObject, mapPos, Quaternion.identity);
        map.SetActive(true);
    }
    private void DeactiveLevel()
    {
        Destroy(map);
    }
    private void OnFinishLevel()
    {
        if (GameManager.Instance.IsState(GameState.ChangeLevel))
        {
            DeactiveLevel();
            levelIndex++;
            ActiveLevel(levelIndex);
            if (levelIndex == 0) // Level 1
            {
                playerTF.position = level1_StartingPosition;
            }
            if (levelIndex == 1) // Level 2
            {
                playerTF.position = level2_StartingPosition;
            }
            if (levelIndex == 2) // Level 3
            {
                OnFinalLevel();
            }
            GameManager.Instance.ChangeState(GameState.GamePlay);
        }
    }
    private void OnFinalLevel()
    {
        SoundManager.Instance.StopIngameSound();
        SoundManager.Instance.PlayBossFightSound();

        Destroy(player);
        player = Instantiate(playerPrefabs[2], level3_StartingPosition, Quaternion.identity);
        playerTF = player.transform;

        boss = Instantiate(bossPrefab, bossPos, Quaternion.identity);
        bossTF = boss.transform;
        UIManager.ChangeUI(bossScenceUI.gameObject);
        bossScenceUI.player = player.GetComponent<Player_Golden>();

        mainCam.SetActive(false);
        finalScenceCam.SetActive(true);
        targetGroup.AddMember(playerTF, 6, 8);
        targetGroup.AddMember(bossTF, 4, 8);
    }
}
