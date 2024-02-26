using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class ThrowDropper : Base_InputSystem
{

    [SerializeField] float _minHeight = 1;
    [SerializeField] float _maxHeight = 10;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] NextYarn _yarnChooser;
    [SerializeField] float _coolDownLength = 0.7f;
    [SerializeField] float _forceDamping = 10;

    [SerializeField] Camera _mainCam;
    [SerializeField] bool _inDoubleClickMode = true;
    [SerializeField] bool _inSlingshotMode = true;
    private bool _isHeld = false;
    private bool _onCoolDown = false;
    private GameObject _currentYarn = null;
    private Rigidbody _currentYarnRb = null;


    public Vector3 TosserPosition { get; private set; } = Vector3.zero;
    public LayerMask ThrowerMask => _layerMask;
    public bool IsHolding => _isHeld;
    // TODO: Replace with actual yarn color
    public Color YarnColor { get; private set; } = Color.white;
    public int SlingshotDirection => _inSlingshotMode ? -1 : 1;

    /// <summary>
    /// This value is the center point from where the mouse is held
    /// </summary>
    public Vector2 CenterMouse { get; private set; }
    /// <summary>
    /// This value tells others where the ball will spawn from when ready to throw
    /// </summary>
    public Vector3 YarnSpawnPosition { get; private set; }

    public float CameraYRotation => _mainCam.transform.rotation.eulerAngles.y;
    // Yarn is perfect sphere (scale: x = y = z) and scale gives diameter
    public float YarnRadius => _currentYarn != null ? _currentYarn.transform.localScale.x / 2 : 0.5f;
    public float YarnMass => _currentYarnRb != null ? _currentYarnRb.mass : 1;
    public float YarnDrag => _currentYarnRb != null ? _currentYarnRb.drag : 1;

    void Start()
    {
        if (!_mainCam) _mainCam = Camera.main;
        if (!_yarnChooser) _yarnChooser = GetComponent<NextYarn>();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Fire.started += Fire_started;
        _input.Player.Fire.canceled += Fire_canceled;
        _input.Player.Cancel.performed += Cancel_performed;
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        _input.Player.Fire.started -= Fire_started;
        _input.Player.Fire.canceled -= Fire_canceled;
        _input.Player.Cancel.performed -= Cancel_performed;
    }

    private void AssignCurrent()
    {
        _currentYarn = Instantiate(_yarnChooser.GetCurrent(), transform.position, transform.rotation);
        _currentYarn.SetActive(false);
        _currentYarnRb = _currentYarn.GetComponent<Rigidbody>();
    }

    private void StartHolding()
    {
        AssignCurrent();
        YarnColor = _currentYarn.GetComponent<MeshRenderer>().sharedMaterial.color;
        _isHeld = true;
        CenterMouse = Mouse.current.position.ReadValue();
        YarnSpawnPosition = transform.position;
    }

    private void ThrowYarn()
    {
        _currentYarn.SetActive(true);
        // Apply calculated force
        _currentYarn.GetComponent<Rigidbody>().AddForce(CalculateTossForce(), ForceMode.Impulse);
        // Remove current
        (_currentYarnRb, _currentYarn) = (null, null);
        _yarnChooser.ChooseNextYarn();
        YarnColor = _yarnChooser.GetCurrent().GetComponent<MeshRenderer>().sharedMaterial.color;
        _isHeld = false;
        StartCoroutine(RunCoolDown());
    }

    private void Fire_started(InputAction.CallbackContext obj)
    {
        if (_onCoolDown)
        {
            return;
        }
        if (_inDoubleClickMode && _isHeld)
        {
            // Throw
            ThrowYarn();
        }
        else
        {
            // Toggling hold on and store mouse center
            StartHolding();
        }

    }

    private void Fire_canceled(InputAction.CallbackContext context)
    {
        if (!_inDoubleClickMode && _isHeld)
        {
            // Throw
            ThrowYarn();
        }
    }

    private void Cancel_performed(InputAction.CallbackContext context)
    {
        // Regardless of double click mode,
        // immediately stop holding
        _isHeld = false;

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
    /// Helper Math function (may be moved out of this class)
    /// </summary>
    /// <param name="vector">Vector to rotate</param>
    /// <param name="degrees">The degree angle to rotate by</param>
    /// <returns></returns>
    public static Vector2 Vec2AngleRotate(Vector2 vector, float degrees)
    {
        float x = vector.x;
        float y = vector.y;

        float angle = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        Vector2 newVec = new Vector2((cos * x) - (sin * y), (sin * x) + (cos * y));

        // Debug.LogWarning($"{vector} + {degrees}° => {newVec}");
        return newVec;
    }

    /// <summary>
    /// Calculate the power and direction of the toss
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateTossForce()
    {
        // Get where the mouse started holding from
        Vector2 mouseScreenCenter = CenterMouse;
        // Get where the mouse currently is
        Vector2 mouseScreenOffset = Mouse.current.position.ReadValue();
        // Find the reflected vector (dampened by some value)
        Vector2 mouseInfo = (mouseScreenOffset - mouseScreenCenter) / _forceDamping;
        // Rotate by the camera's "y" rotation Euler value (depending on the angle)
        mouseInfo = SlingshotDirection * Vec2AngleRotate(mouseInfo, -CameraYRotation);
        return new(mouseInfo.x, 0, mouseInfo.y);
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
