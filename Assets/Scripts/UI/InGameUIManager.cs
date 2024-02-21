using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject _pauseUI;
    [SerializeField] GameObject _settingsUI;
    [SerializeField] GameObject _confirmationUI;
    [SerializeField] GameObject _gameOverUI;

    void Start()
    {
        if (!_gameManager) _gameManager = FindObjectOfType<GameManager>();
        _pauseUI.SetActive(false);
        _settingsUI.SetActive(false);
        _confirmationUI.SetActive(false);
        _gameOverUI.SetActive(false);
    }

    // TEMP: This should be replaced by InputAction event
    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Time.timeScale == 0)
            {
                OnResumeGame();
            }
            else
            {
                OnPauseGame();
            }
        }
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
        AkSoundEngine.SetState("GameStates", "IngameState");
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
       
    }
}
