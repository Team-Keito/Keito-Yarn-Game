using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultInputMode : MonoBehaviour
{
    [SerializeField] private ControlMap _defaultInputMode = ControlMap.None;

    void Start()
    {
        InputManager.SwitchControls(_defaultInputMode);
    }
}
