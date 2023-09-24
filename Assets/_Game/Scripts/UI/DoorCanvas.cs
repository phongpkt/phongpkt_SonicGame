using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorCanvas : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject text;

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
