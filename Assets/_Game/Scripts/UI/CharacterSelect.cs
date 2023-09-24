using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public void ChangeCharacter (int selectedCharacter)
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
    }
}
