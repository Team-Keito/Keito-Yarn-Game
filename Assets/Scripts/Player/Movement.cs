using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 5f;

    [SerializeField]
    private float _rotationSpeed = 5f;

    [SerializeField] private float _clampVertical = 85f;

    [SerializeField] private GameObject _camera;

    private Vector2 _currentRotation = new Vector2();

    private IA_Controls inputActions;

    private void Awake()
    {
        inputActions = new IA_Controls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// Camera look is sorta buggy when loading 
    /// TODO: fix possibly skip first few updates?
    /// </summary>
    private void Update()
    {
        UpdateMove();
        UpdateLook();
    }

    private void UpdateMove()
    {
        Vector3 movement = inputActions.Move.Movement.ReadValue<Vector3>();

        movement = transform.forward * movement.z + transform.right * movement.x + transform.up * movement.y;

        transform.position += _movementSpeed * Time.deltaTime * movement;
    }

    private void UpdateLook()
    {
        Vector2 mouseMovement = inputActions.Move.Look.ReadValue<Vector2>();

        _currentRotation.y += mouseMovement.x * Time.deltaTime * _rotationSpeed;
        _currentRotation.x += -mouseMovement.y * Time.deltaTime * _rotationSpeed;

        _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_clampVertical, _clampVertical);

        _camera.transform.rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(0, _currentRotation.y, 0);
    }
}
