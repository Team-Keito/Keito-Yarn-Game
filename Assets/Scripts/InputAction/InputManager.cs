using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public ControlMap Current;
    public static PlayerControls Input;

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

        if (Input == null)
        {
            Input = new PlayerControls();
        }
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    public void SwitchControls(ControlMap map)
    {
        Current = map;
        Input.Disable();
        switch (map)
        {
            case ControlMap.Player:
                Input.Player.Enable();
                break;

            case ControlMap.UI:
                Input.UI.Enable();
                break;
        }
    }
}


public enum ControlMap {
    None,
    Player,
    UI,
}