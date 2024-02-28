using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject _pauseUI;
    [SerializeField] GameObject _settingsUI;
    [SerializeField] GameObject _confirmationUI;
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] PlayerPrefSO masterSO, musicSO, soundSO, _BestTimePlayerPref;
    [SerializeField] private TextMeshProUGUI endTimeText, bestTimeText, currTimeText;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private GameObject scoreColor;

    void Start()
    {
        if (!_gameManager) _gameManager = FindObjectOfType<GameManager>();
        _pauseUI.SetActive(false);
        _settingsUI.SetActive(false);
        _confirmationUI.SetActive(false);
        _gameOverUI.SetActive(false);

        if (currTimeText)
        {
            InvokeRepeating("Timer", 1f, _gameManager.TimePerSecond);
        }
        else
        {
            Debug.LogWarning("Missing UI Reference: Timer");
        }
    }

    private void OnEnable()
    {
        InputManager.Input.Player.Menu.performed += Menu_performed;
        InputManager.Input.UI.Cancel.performed += Cancel_performed;
    }

    private void OnDisable()
    {
        InputManager.Input.Player.Menu.performed -= Menu_performed;
        InputManager.Input.UI.Cancel.performed -= Cancel_performed;
    }

    private void Cancel_performed(InputAction.CallbackContext obj)
    {
        if (_settingsUI.activeSelf)
        {
            OnSettingsClose();
        }
        else if (_confirmationUI.activeSelf)
        {
            OnCloseConfirmation();
        }
        else
        {
            OnResumeGame();
        }
    }

    private void Menu_performed(InputAction.CallbackContext obj)
    {
        OnPauseGame();
    }

    public void OnPauseGame()
    {
        _pauseUI.SetActive(true);
        _gameManager.PauseGame();
        AkSoundEngine.SetState("GameStates", "Pause_State");
    }

    public void OnResetGame()
    {
        _gameOverUI.SetActive(false);
        _gameManager.ResumeGame();
        _gameManager.RestartGame();
    }

    public void OnResumeGame()
    {
        _gameManager.ResumeGame();
        AkSoundEngine.SetState("GameStates", "IngameState");
        _pauseUI.SetActive(false);
    }

    public void OnSettingsOpen()
    {
        _settingsUI.SetActive(true);
        _pauseUI.SetActive(false);
    }

    public void OnSettingsClose()
    {
        _pauseUI.SetActive(true);
        _settingsUI.SetActive(false);
    }

    public void OnOpenConfirmation()
    {
        _confirmationUI.SetActive(true);
        _pauseUI.SetActive(false);

    }

    public void OnCloseConfirmation()
    {
        _pauseUI.SetActive(true);
        _confirmationUI.SetActive(false);
    }

    public void OnBackToMainMenu()
    {
        _gameManager.ResumeGame();
        _gameManager.LoadMainMenu();
    }

    public void OnGameEnd()
    {
        _gameManager.PauseGame();
        _gameOverUI.SetActive(true);
        AkSoundEngine.SetState("GameStates", "Game_over");

    }

    public void OnResetSettings()
    {
        PlayerPrefs.DeleteKey(masterSO.currKey.ToString());
        PlayerPrefs.DeleteKey(musicSO.currKey.ToString());
        PlayerPrefs.DeleteKey(soundSO.currKey.ToString());
    }

    /// <summary>
    /// Timer for counting how much time has passed. Also records best time if it is less than current time.
    /// </summary>
    public void Timer()
    {
        _gameManager.CurrentTime++;
        currTimeText.text = System.TimeSpan.FromSeconds(_gameManager.CurrentTime).ToString(@"m\:ss");

        if (_gameManager.Score >= _gameManager.TargetScore)
        {
            _gameManager.BestTime = _gameManager.CurrentTime;

            PlayerPrefs.SetInt(_BestTimePlayerPref.currKey.ToString(), _gameManager.BestTime);
            PlayerPrefs.Save();

            CancelInvoke("Timer");

            endTimeText.text = string.Format("Final Time: {0}", _gameManager.CurrentTime);
            bestTimeText.text = string.Format("Best Time: {0}", _gameManager.BestTime);

            _gameManager.OnGameEnd.Invoke();
        }
    }

    public void ChangeProgressBar()
    {
        if (scoreSlider)
        {
            scoreSlider.value = _gameManager.Score / _gameManager.TargetScore;

            if (scoreSlider.value < 0.25f)
                scoreColor.GetComponent<Image>().color = Color.red;
            else if (scoreSlider.value > 0.25f && scoreSlider.value < 0.75f)
                scoreColor.GetComponent<Image>().color = Color.yellow;
            else if (scoreSlider.value > 0.75f && scoreSlider.value < 0.99f)
                scoreColor.GetComponent<Image>().color = Color.green;
            else
                scoreColor.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            Debug.LogWarning("Missing UI Reference: Score Slider");
        }
    }
}
