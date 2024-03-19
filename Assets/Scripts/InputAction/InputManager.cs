using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public ControlMap Default;

    public static System.Action<string> OnChangeScheme;
    public UnityEvent<string> OnChangeControl;

    public static string CurrentControlScheme { get; private set; }

    private static PlayerControls _input;
    public static PlayerControls Input { get => _input != null ? _input : (_input = new PlayerControls()); }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        SwitchControls(Default);
    }

    public void OnControlsChanged(PlayerInput obj)
    {
        CurrentControlScheme = obj.currentControlScheme;

        OnChangeScheme?.Invoke(CurrentControlScheme);
        OnChangeControl.Invoke(CurrentControlScheme);
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()    {
        Input.Disable();
    }

    public static void SwitchControls(ControlMap map)
    {
        Input.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        switch (map)
        {
            case ControlMap.Player:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Input.Player.Enable();
                break;

                
            case ControlMap.UI:
                Input.UI.Enable();
                break;

            case ControlMap.None:
                break;

            default:
                Input.Enable();
                break;
        }
    }
}


public enum ControlMap {
    All,
    Player,
    UI,
    None,
}