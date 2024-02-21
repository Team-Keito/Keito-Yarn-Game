using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusChecker : MonoBehaviour
{
    private void Start()
    {
        InputManager.Input.Player.Focus.performed += Focus_performed;
    }

    private void Focus_performed(InputAction.CallbackContext obj)
    {
        if (Time.timeScale == 0)
            return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray mouseRay = Camera.main.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
        {
            hit.collider.gameObject.GetComponent<CameraSwitch>()?.Focus();    
        }
    }
}
