using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Transform bossTF;
    private GameObject player;
    private Transform playerTF;

    //Level
    private Vector3 level1_StartingPosition = new Vector3(-48.5f, 0, 0);
    private Vector3 level2_StartingPosition = new Vector3(-45, 2.5f, 0);
    private Vector3 level3_StartingPosition = new Vector3(115, 50, 0);
    public List<Map> levelMap = new List<Map>();
    [SerializeField] private int levelIndex;

    //Player Prefabs
    public CinemachineVirtualCamera vCam;
    public GameObject mainCam;

    public GameObject finalScenceVCam;
    public CinemachineTargetGroup targetGroup;

    public GameObject[] playerPrefabs;
    private int characterIndex;

    [SerializeField] private UIManager UIManager;
    [SerializeField] private InGameUI inGameUI;
    [SerializeField] private BossFightUI bossScenceUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    //private void Awake()
    //{
    //    OnInit();
    //}
    //private void OnInit()
    //{
    //    levelIndex = 0;
    //}
    private void Update()
    {
        OnFinishLevel();
        OnLose();
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
    }
    public void Replay()
    {
        levelIndex = 0;
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
            DeactiveLevel(levelIndex);
            UIManager.ChangeUI(loseUI);
        }
    }
    private void ActiveLevel(int level)
    {
        levelMap[level].gameObject.SetActive(true);
    }
    private void DeactiveLevel(int level)
    {
        levelMap[level].gameObject.SetActive(false);
    }
    private void OnFinishLevel()
    {
        if (GameManager.Instance.IsState(GameState.GameWin))
        {
            DeactiveLevel(levelIndex);
            levelIndex++;
            ActiveLevel(levelIndex);
            if (levelIndex == 1) // Level 2
            {
                playerTF.position = level2_StartingPosition;
            }
            GameManager.Instance.ChangeState(GameState.GamePlay);
        }
    }
    public void OnFinalLevel()
    {
        Destroy(player);
        player = Instantiate(playerPrefabs[2], level3_StartingPosition, Quaternion.identity);
        playerTF = player.transform;
        bossScenceUI.player = player.GetComponent<Player_Golden>();
        mainCam.SetActive(false);
        finalScenceVCam.SetActive(true);
        targetGroup.AddMember(playerTF, 5, 6);
        targetGroup.AddMember(bossTF, 4, 6);
        GameManager.Instance.ChangeState(GameState.GameWin);
        OnFinishLevel();
    }
}
