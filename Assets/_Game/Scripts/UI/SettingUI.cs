using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private GameObject musicOn;
    [SerializeField] private GameObject musicOff;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Save();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    public void TurnMusicOn()
    {
        musicOn.SetActive(true);
        musicOff.SetActive(false);
    }
    public void TurnMusicOff()
    {
        musicOn.SetActive(false);
        musicOff.SetActive(true);
    }
    public void TurnSoundOn()
    {
        soundOn.SetActive(true);
        soundOff.SetActive(false);
    }
    public void TurnSoundOff()
    {
        soundOn.SetActive(false);
        soundOff.SetActive(true);
    }
}
