using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class ThrowDropper : Base_InputSystem
{

    [SerializeField] float _minHeight = 1;
    [SerializeField] float _maxHeight = 10;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _coolDownLength = 0.7f;

    [SerializeField] Camera _mainCam;
    private bool _isHeld = false;
    private bool _onCoolDown = false;


    public Vector3 TosserPosition { get; private set; } = Vector3.zero;
    public LayerMask ThrowerMask => _layerMask;
    public bool IsHolding => _isHeld;
    // TODO: Replace with actual yarn color
    public Color YarnColor => _isHeld ? Color.magenta : Color.white;

    /// <summary>
    /// This value is the center point from where the mouse is held
    /// </summary>
    public Vector2 CenterMouse { get; private set; }
    /// <summary>
    /// This value tells others where the ball will spawn from when ready to throw
    /// </summary>
    public Vector3 YarnSpawnPosition { get; private set; }

    public float CameraYRotation => _mainCam.transform.rotation.eulerAngles.y;

    void Start()
    {
        if (!_mainCam) _mainCam = Camera.main;
        _input.Player.Fire.started += Fire_started;
    }

    private void Fire_started(InputAction.CallbackContext obj)
    {
        if (_onCoolDown)
        {
            return;
        }
        if (_isHeld)
        {
            // Throw
            StartCoroutine(RunCoolDown());
            _isHeld = false;
        }
        else
        {
            // Toggling hold on and store mouse center
            _isHeld = true;
            CenterMouse = Mouse.current.position.ReadValue();
            YarnSpawnPosition = transform.position;
        }

    }


    public void Update()
    {
        if (!_isHeld)
        {
            TosserPosition = CalculatePosition() + Vector3.up * _maxHeight;
            transform.position = TosserPosition;
        }
    }

    /// <summary>
    /// Calculate where in the level to place the tosser
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculatePosition()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray mouseRay = _mainCam.ScreenPointToRay(mouseScreenPosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            // Calculate dropper upwards from where on the floor it was hit from
            return hit.point;
        }
        else
        {
            // Default to current position
            return transform.position;
        }
    }

    /// <summary>
    /// Calculate how much power for a toss
    /// </summary>
    /// <returns></returns>
    public float CalculateTossPower()
    {
        return 0;
    }

    /// <summary>
    /// Calculate what direction the toss will be in
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateTossDirection()
    {
        return Vector3.zero;
    }


    /// <summary>
    /// Coroutine for waiting between drops. Currently based on wait time
    /// </summary>
    /// <returns></returns>
    IEnumerator RunCoolDown()
    {
        _onCoolDown = true;
        yield return new WaitForSeconds(_coolDownLength);
        _onCoolDown = false;
    }
}
