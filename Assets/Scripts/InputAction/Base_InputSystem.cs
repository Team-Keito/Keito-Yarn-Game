using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Base_InputSystem : MonoBehaviour
{
    protected PlayerControls _input => InputManager.Input;
}