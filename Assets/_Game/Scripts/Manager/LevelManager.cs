using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LevelManager : Singleton<LevelManager>
{
    public Character player;
    public Vector3 startingPosition = new Vector3(-45, 2.5f, 0);
    public List<Map> levelMap = new List<Map>();
    private int levelIndex;
    private void Awake()
    {
        OnInit();
    }
    private void OnInit()
    {
        levelIndex = 0;
    }
    private void Start()
    {
        ActiveLevel(levelIndex);
    }
    private void Update()
    {
        OnFinishGame();
    }
    private void ActiveLevel(int level)
    {
        levelMap[level].gameObject.SetActive(true);
        Debug.Log("Load Level: " + level);
        Debug.Log(levelMap[level].gameObject);
    }
    private void DeactiveLevel(int level)
    {
        levelMap[level].gameObject.SetActive(false);
        Debug.Log("Destroy Level: " + level);
        Debug.Log(levelMap[level].gameObject);
    }
    private void OnPlay()
    {

    }
    private void OnFinishGame()
    {
        if (GameManager.Instance.IsState(GameState.GameWin))
        {
            DeactiveLevel(levelIndex);
            levelIndex++;
            ActiveLevel(levelIndex);
            player.transform.position = startingPosition;
            GameManager.Instance.ChangeState(GameState.GamePlay);
        }
    }
}
