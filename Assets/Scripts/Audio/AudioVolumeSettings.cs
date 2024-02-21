using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSettings : MonoBehaviour
{
    public string wwiseMasterRTPCName = "MasterVolume";
    public Slider masterVolumeSlider;
    public PlayerPrefSO masterSO;

    public string wwiseMusicRTPCName = "MusicVolume";
    public Slider musicVolumeSlider;
    public PlayerPrefSO musicSO;

    public string wwiseSFXRTPCName = "SFXVolume";
    public Slider sfxVolumeSlider;
    public PlayerPrefSO soundSO;

    private void Start()
    {
        // Sets the volumes to their defaults in the editor if the key for the volume types isn't there
        float masterVol = PlayerPrefs.HasKey(masterSO.currKey.ToString()) ? PlayerPrefs.GetFloat(masterSO.currKey.ToString()) : masterVolumeSlider.value;
        float musicVol = PlayerPrefs.HasKey(musicSO.currKey.ToString()) ? PlayerPrefs.GetFloat(masterSO.currKey.ToString()) : musicVolumeSlider.value;
        float soundVol = PlayerPrefs.HasKey(soundSO.currKey.ToString()) ? PlayerPrefs.GetFloat(masterSO.currKey.ToString()) : sfxVolumeSlider.value;

        AkSoundEngine.SetRTPCValue(wwiseMasterRTPCName, masterVol);
        AkSoundEngine.SetRTPCValue(wwiseMusicRTPCName, musicVol);
        AkSoundEngine.SetRTPCValue(wwiseSFXRTPCName, soundVol);

        masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
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