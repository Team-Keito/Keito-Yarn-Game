using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoSelect : MonoBehaviour
{
    [SerializeField] private Selectable _selectTarget;
    [SerializeField] private GameObject _cancelTarget;


    private void OnEnable()
    {
        _selectTarget.Select();
        InputManager.Input.UI.Cancel.performed += Cancel_performed;
    }

    private void OnDisable()
    {
        InputManager.Input.UI.Cancel.performed -= Cancel_performed;
    }

    private void Cancel_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        Debug.Log("Cancel");
        if (_cancelTarget != null)
        {
            _cancelTarget.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
