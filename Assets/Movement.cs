using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float _rotationSpeedX = 5f;

    [SerializeField] private float _clampVertical = 85f;

    [SerializeField] private GameObject _camera;

    private Vector3 _movement;
    private Vector2 _mouseMovement;
    private float _mouseYRotation = 0f;

    private IA_Controls inputActions;


    private void Awake()
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

    private void Update()
    {
        transform.position += inputActions.Move.Movement.ReadValue<Vector3>() * speed * Time.deltaTime;

        Vector2 mouseMovement = inputActions.Move.Look.ReadValue<Vector2>();

        //Camera movement delta is buggy when loading.
        //probably delta frame extends loading bar duration?
/*        
        transform.Rotate(Vector3.up, mouseMovement.x * Time.deltaTime * _rotationSpeedX);


        _mouseYRotation -= mouseMovement.y * Time.deltaTime * _rotationSpeedX;
        _mouseYRotation = Mathf.Clamp(_mouseYRotation, -_clampVertical, _clampVertical);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = _mouseYRotation;
        _camera.transform.eulerAngles = targetRotation;*/
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        _movement = context.ReadValue<Vector3>();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        _mouseMovement = context.ReadValue<Vector2>();
    }
}
