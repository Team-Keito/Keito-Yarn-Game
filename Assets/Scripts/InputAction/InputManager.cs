using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class InputManager: ScriptableObject
{
    public static PlayerControls Input;

    private void Start()
    {
        Input = new PlayerControls();
    }
}
