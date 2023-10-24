using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject inGameUI;
    public void ChangeToMainMenu()
    {
        GameManager.Instance.ChangeState(GameState.MainMenu);
        UIManager.Instance.ChangeUI(mainMenu);
        gameObject.SetActive(false);
    }
    public void Replay()
    {
        LevelManager.Instance.Replay();
        UIManager.Instance.ChangeUI(inGameUI);
        gameObject.SetActive(false);
    }
}
