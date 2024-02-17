using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSettings : MonoBehaviour
{
    public string wwiseMasterRTPCName = "MasterVolume";
    public Slider masterVolumeSlider;

    public string wwiseMusicRTPCName = "MusicVolume";
    public Slider musicVolumeSlider;

    public string wwiseSFXRTPCName = "SFXVolume";
    public Slider sfxVolumeSlider;

    private void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    private void UpdateMasterVolume(float value)
    {
        AkSoundEngine.SetRTPCValue(wwiseMasterRTPCName, value);
    }

    private void UpdateMusicVolume(float value1)
    {
        AkSoundEngine.SetRTPCValue(wwiseMusicRTPCName, value1);
    }

    private void UpdateSFXVolume(float value2)
    {
        AkSoundEngine.SetRTPCValue(wwiseSFXRTPCName, value2);
    }

}