using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSettings : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public TextMeshProUGUI mouseSensitivityText;
    public PlayerPrefSO mouseSO;
    private float mouseSensitivity = 50f;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        mouseSensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp()
    {
        if (PlayerPrefs.HasKey(mouseSO.currKey.ToString()))
        {
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat(mouseSO.currKey.ToString());
            mouseSensitivityText.text = PlayerPrefs.GetFloat(mouseSO.currKey.ToString()).ToString();

            AdjustSensitivity(mouseSensitivitySlider.value);
        }
    }

    private void UpdateMouseSensitivity(float value)
    {
        AdjustSensitivity(value);

        mouseSensitivityText.text = value.ToString();

        PlayerPrefs.SetFloat(mouseSO.currKey.ToString(), value);
        PlayerPrefs.Save();
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey(mouseSO.currKey.ToString());
        mouseSensitivitySlider.value = mouseSensitivity;
        mouseSensitivityText.text = mouseSensitivity.ToString();
    }

    private void AdjustSensitivity(float value)
    {
        if (SlingShot.Instance)
        {
            SlingShot.Instance.mouseX = (value / mouseSensitivitySlider.maxValue) * Mathf.Sqrt(mouseSensitivitySlider.maxValue);
            SlingShot.Instance.mouseY = (value / mouseSensitivitySlider.maxValue) * Mathf.Sqrt(mouseSensitivitySlider.maxValue);
        }
    }
}
