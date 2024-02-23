using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public ControlMap Default;

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