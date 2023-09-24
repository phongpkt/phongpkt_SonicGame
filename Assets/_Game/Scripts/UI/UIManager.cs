using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //[SerializeField] private GameObject inGameUI;
    //[SerializeField] private GameObject mainMenuUI;
    //[SerializeField] private GameObject bossFightUI;
    //[SerializeField] private GameObject winUI;
    //[SerializeField] private GameObject loseUI;
    //[SerializeField] private GameObject settingUI;
    //[SerializeField] private GameObject scoreboardUI;
    //[SerializeField] private GameObject chooseCharacterUI;

    [SerializeField] private GameObject openScenceUI;
    private GameObject currentUI;

    private void Start()
    {
        openScenceUI.SetActive(true);
        currentUI = openScenceUI;
    }
    public void ChangeUI(GameObject _currentUI)
    {
        currentUI.SetActive(false);
        currentUI = _currentUI;
        currentUI.SetActive(true);
    }
}
