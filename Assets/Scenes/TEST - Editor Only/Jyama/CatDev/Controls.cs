using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rotationSpeed = 15;
    [SerializeField] private float _moveSpeed = 15;
    [SerializeField] private float _clampVertical = 85;
    private Vector2 _currentRotation = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        if (!_camera) _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Move = InputManager.Input.Player.Look.ReadValue<Vector2>();

        Vector3 movement = transform.forward * Move.y + transform.right * Move.x;
        transform.position += movement * Time.deltaTime * _moveSpeed;

        UpdateLook();
    }

    private void UpdateLook()
    {
        Vector2 mouseMovement = InputManager.Input.Player.Move.ReadValue<Vector2>();

        _currentRotation.y += mouseMovement.x * Time.deltaTime * _rotationSpeed;
        _currentRotation.x += -mouseMovement.y * Time.deltaTime * _rotationSpeed;

        _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_clampVertical, _clampVertical);

        _camera.transform.rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        transform.rotation = Quaternion.Euler(0, _currentRotation.y, 0);
    }
}
