using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Base_InputSystem : MonoBehaviour
{
    protected PlayerControls _input;

    // Start is called before the first frame update
    void Awake()
    {
        _input = new PlayerControls();
    }

    private void OnValidate()
    {
        if (_input == null)
            _input = new PlayerControls();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
