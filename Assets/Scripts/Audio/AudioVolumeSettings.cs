using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSettings : MonoBehaviour
{
    public string wwiseMasterRTPCName = "MasterVolume";
    public Slider masterVolumeSlider;
    public PlayerPrefSO masterSO;
    private float masterVol = 50f;

    public string wwiseMusicRTPCName = "MusicVolume";
    public Slider musicVolumeSlider;
    public PlayerPrefSO musicSO;
    private float musicVol = 50f;

    public string wwiseSFXRTPCName = "SFXVolume";
    public Slider sfxVolumeSlider;
    public PlayerPrefSO soundSO;
    private float soundVol = 50f;

    private void Start()
    {
        SetUp();

        masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    public void SetUp()
    {
        if (PlayerPrefs.HasKey(masterSO.currKey.ToString()))
            masterVolumeSlider.value = PlayerPrefs.GetFloat(masterSO.currKey.ToString());

        if (PlayerPrefs.HasKey(musicSO.currKey.ToString()))
            musicVolumeSlider.value = PlayerPrefs.GetFloat(musicSO.currKey.ToString());

        if (PlayerPrefs.HasKey(soundSO.currKey.ToString()))
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(soundSO.currKey.ToString());

        AkSoundEngine.SetRTPCValue(wwiseMasterRTPCName, masterVolumeSlider.value);
        AkSoundEngine.SetRTPCValue(wwiseMusicRTPCName, musicVolumeSlider.value);
        AkSoundEngine.SetRTPCValue(wwiseSFXRTPCName, sfxVolumeSlider.value);
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey(masterSO.currKey.ToString());
        masterVolumeSlider.value = masterVol;
        
        PlayerPrefs.DeleteKey(musicSO.currKey.ToString());
        musicVolumeSlider.value = musicVol;

        PlayerPrefs.DeleteKey(soundSO.currKey.ToString());
        sfxVolumeSlider.value = soundVol;
    }

    private void UpdateMasterVolume(float value)
    {
        AkSoundEngine.SetRTPCValue(wwiseMasterRTPCName, value);

        PlayerPrefs.SetFloat(masterSO.currKey.ToString(), value);
        PlayerPrefs.Save();
    }

    private void UpdateMusicVolume(float value1)
    {
        AkSoundEngine.SetRTPCValue(wwiseMusicRTPCName, value1);

        PlayerPrefs.SetFloat(musicSO.currKey.ToString(), value1);
        PlayerPrefs.Save();
    }

    private void UpdateSFXVolume(float value2)
    {
        AkSoundEngine.SetRTPCValue(wwiseSFXRTPCName, value2);

        PlayerPrefs.SetFloat(soundSO.currKey.ToString(), value2);
        PlayerPrefs.Save();
    }

}