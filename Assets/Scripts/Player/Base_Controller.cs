using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Base_Controller : MonoBehaviour
{
    private IA_Controls inputActions;

    void Start()
    {
        inputActions = new IA_Controls();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
