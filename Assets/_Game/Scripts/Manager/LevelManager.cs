using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    [SerializeField] private Player player;
    private Transform bossTF;
    private Transform playerTF;
    private int characterIndex;
    private Vector3 bossPos = new Vector3(130f, 50f, 0);

    //UI
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private BossFightUI bossScenceUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    private void Awake()
    {
        OnInit();
        finalScenceCam.SetActive(false);
    }
    private void OnInit()
    {
        levelIndex = 0;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        finalScenceCam = GameObject.FindGameObjectWithTag("FinalScenceCamera");
    }
    private void Update()
    {
        OnFinishLevel();
    }
    public void OnPlayerDied()
    {
        PlayerPrefs.SetInt("coin", player.coin);
        GameManager.Instance.ChangeState(GameState.GameOver);
        OnLose();
    }
    public void OnPlay()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter");
        ActiveLevel(levelIndex);
        GameObject temp = PhotonNetwork.Instantiate(playerPrefabs[characterIndex].name, level1_StartingPosition, Quaternion.identity);
        player = temp.GetComponent<Player>();
        if(player != null)
        {
            playerTF = temp.transform;
            vCam.m_Follow = temp.transform;
            inGameUI.player = player;
        }
        SoundManager.Instance.PlayIngameSound();
    }
    public void Replay()
    {
        levelIndex = 0;
        finalScenceCam.SetActive(false);
        mainCam.SetActive(true);
        OnPlay();
    }
    private void OnLose()
    {
        if (GameManager.Instance.IsState(GameState.GameOver))
        {
            SoundManager.Instance.StopIngameSound();
            SoundManager.Instance.StopBossFightSound();
            SoundManager.Instance.PlayWhenLose();

            Destroy(player.gameObject);
            DeactiveLevel();
            playerTF = null;
            vCam.m_Follow = null;
            inGameUI.player = null;
            UIManager.Instance.ChangeUI(loseUI);

            //Save score
            //int coin = PlayerPrefs.GetInt("coin");
            //string time = inGameUI.currentTime;
            leaderboardUI.AddHighScoreEntry(inGameUI.currentTime, PlayerPrefs.GetInt("coin"));

            //GameManager.Instance.ChangeState(GameState.GamePause);
        }
    }
    public void Victory(Boss b)
    {
        if (b.isDead)
        {
            PlayerPrefs.SetInt("coin", player.coin);
            GameManager.Instance.ChangeState(GameState.GameWin);
            OnWin();
        }
    }
    private void OnWin()
    {
        if (GameManager.Instance.IsState(GameState.GameWin))
        {
            SoundManager.Instance.StopIngameSound();
            SoundManager.Instance.StopBossFightSound();
            SoundManager.Instance.PlayWhenWin();

            Destroy(player.gameObject);
            DeactiveLevel();
            playerTF = null;
            vCam.m_Follow = null;
            inGameUI.player = null;
            UIManager.Instance.ChangeUI(winUI);

            //Save score
            int coin = PlayerPrefs.GetInt("coin");
            string time = inGameUI.currentTime;
            leaderboardUI.AddHighScoreEntry(time, coin);
            //GameManager.Instance.ChangeState(GameState.GamePause);
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

        Destroy(player.gameObject);
        GameObject golden = PhotonNetwork.Instantiate(playerPrefabs[2].name, level3_StartingPosition, Quaternion.identity);
        Player_Golden p = golden.GetComponent<Player_Golden>();

        if (p != null) 
        {
            playerTF = golden.transform;
            bossScenceUI.player = p;
        }

        boss = Instantiate(bossPrefab, bossPos, Quaternion.identity);
        bossTF = boss.transform;
        UIManager.Instance.ChangeUI(bossScenceUI.gameObject);

        mainCam.SetActive(false);
        finalScenceCam.SetActive(true);
        targetGroup.AddMember(playerTF, 6, 8);
        targetGroup.AddMember(bossTF, 4, 8);
    }
}
