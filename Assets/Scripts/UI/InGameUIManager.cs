using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject _pauseUI;
    [SerializeField] GameObject _settingsUI;
    [SerializeField] GameObject _confirmationUI;
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] PlayerPrefSO masterSO, musicSO, soundSO;

    void Start()
    {
        if (!_gameManager) _gameManager = FindObjectOfType<GameManager>();
        _pauseUI.SetActive(false);
        _settingsUI.SetActive(false);
        _confirmationUI.SetActive(false);
        _gameOverUI.SetActive(false);

        InputManager.Input.Player.Menu.performed += Menu_performed;
        InputManager.Input.UI.Cancel.performed += Cancel_performed;
    }

    private void Cancel_performed(InputAction.CallbackContext obj)
    {
        if (_settingsUI.activeSelf)
        {
            OnSettingsClose();
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
        InputManager.Instance.SwitchControls(ControlMap.UI);
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

        InputManager.Instance.SwitchControls(ControlMap.Player);
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
       
    }

    public void OnResetSettings()
    {
        PlayerPrefs.DeleteKey(masterSO.currKey.ToString());
        PlayerPrefs.DeleteKey(musicSO.currKey.ToString());
        PlayerPrefs.DeleteKey(soundSO.currKey.ToString());
    }
}
