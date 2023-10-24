using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    //Sounds
    [SerializeField] private AudioSource inGameSource;
    [SerializeField] private AudioSource bossFightSource;
    [SerializeField] private AudioSource loseSource;
    [SerializeField] private AudioSource winSource;

    [SerializeField] private AudioSource btn_ClickSource;
    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
    }
    public void PlayIngameSound()
    {
        inGameSource.Play();
    }
    public void StopIngameSound()
    {
        inGameSource.Stop();
    }
    public void PlayBossFightSound()
    {
        bossFightSource.Play();
    }
    public void StopBossFightSound()
    {
        bossFightSource.Stop();
    }
    public void PlayWhenLose()
    {
        loseSource.Play();
    }
    public void PlayWhenWin()
    {
        winSource.Play();
    }
    public void PlayWhenClick()
    {
        btn_ClickSource.Play();
    }
    public void MuteSound()
    {
        loseSource.mute = true;
        winSource.mute = true;
        btn_ClickSource.mute = true;
    }
    public void UnMuteSound()
    {
        loseSource.mute = false;
        winSource.mute = false;
        btn_ClickSource.mute = false;
    }

    public void MuteMusic()
    {
        bossFightSource.mute = true;
        inGameSource.mute = true;
    }
    public void UnMuteMusic()
    {
        bossFightSource.mute = false;
        inGameSource.mute = false;
    }
}