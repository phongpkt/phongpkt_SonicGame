using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { MainMenu, GamePause, GamePlay, ChangeLevel, BossFight, GameOver, GameWin }
public class GameManager : Singleton<GameManager>
{
    private GameState gameState;
    public void ChangeState(GameState state)
    {
        this.gameState = state;
    }
    public bool IsState(GameState state) => state == this.gameState;

    private void Awake()
    {
        gameState = GameState.MainMenu;
    }
}
