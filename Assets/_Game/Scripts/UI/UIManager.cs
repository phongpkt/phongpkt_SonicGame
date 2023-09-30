using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
