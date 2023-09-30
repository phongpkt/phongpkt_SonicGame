using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorCanvas : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Door door;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject text;

    private void OnEnable()
    {
        btn.onClick.AddListener(ChangeMapEvent);
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
    private void ChangeMapEvent()
    {
        GameManager.Instance.ChangeState(GameState.ChangeLevel);
    }
    private void Update()
    {
        if (door.isLocked)
        {
            button.SetActive(false);
            text.SetActive(true);
        }
        else
        {
            button.SetActive(true);
            text.SetActive(false);
        }
    }

}
