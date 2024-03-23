using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SlingShot : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField] private GameObject _indicator;
    [SerializeField] private NextColor _PrefabPicker;

    [Space(5)]
    [SerializeField] private Vector3 _postionOffset;
    [SerializeField, Tooltip("x & y are flipped cause unity euler")]
    private Vector2 _rotationOffset;

    [SerializeField] private float _afterFireCD = 0.4f;

    [Space(5)]
    [SerializeField] private int _linePoints = 25;
    [SerializeField] private float _totalTime = 2f;
    [SerializeField] private LayerMask _layerMask;

    [Space(5)]
    [SerializeField] private bool _isToggle = false;
    [SerializeField] private AlignmentControl _horizontal;
    [SerializeField] private AlignmentControl _forceVertical;
    
    [SerializeField, Range(0, 1)] private float _startForceMulti = 0.5f;
    #endregion

    #region Events
    //Events:
    public UnityEvent<float> OnPowerChange;
    public UnityEvent OnThrow;
    public UnityEvent OnStartThrow;
    public UnityEvent<Queue<Color>> OnNextColorChange;
    #endregion

    private LineRenderer _lineRenderer;

    private float _force, _mass, _drag, _radius;
    private GameObject _currentBall;

    private bool _isHeld = false;
    private Vector2 _currentRotation;

    private Vector3 _forceVector;
    private bool _onCoolDown;

    private Vector3 StartOffset => CalcOffset(Camera.main.transform, _postionOffset);
    public GameObject CurrentBall => _currentBall;

    public float mouseX { get { return _horizontal.Speed; } set { _horizontal.Speed = value; } } 
    public float mouseY { get { return _forceVertical.Speed; } set { _forceVertical.Speed = value; } }

    public static SlingShot Instance;

    private void Start()
    {
        Instance = this;
        _PrefabPicker.Setup();
        OnNextColorChange.Invoke(GetNextColors());

        _lineRenderer = gameObject.GetComponent<LineRenderer>();

        //Parents object to Camera w/offset (Avoids jittery movement)
        transform.position = CalcOffset(Camera.main.transform, _postionOffset);
        transform.SetParent(Camera.main.transform, true);
    }

    private void OnEnable()
    {
        InputManager.Input.Player.Enable();
        InputManager.Input.Player.Fire.canceled += Fire_completed;
        InputManager.Input.Player.Fire.performed += Fire_performed;

        InputManager.Input.Player.Cancel.performed += Cancel_performed;
    }


    private void OnDisable()
    {
        InputManager.Input.Player.Disable();
        InputManager.Input.Player.Fire.performed -= Fire_performed;
        InputManager.Input.Player.Fire.canceled -= Fire_completed;

        InputManager.Input.Player.Cancel.performed -= Cancel_performed;
    }

    private void Update()
    {
        if (_isHeld)
        {
            UpdateRotation();

            _forceVector = _force * transform.forward;

            DrawWithDrag(_forceVector);
            _indicator.transform.position = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);

            _currentBall.transform.position = StartOffset;
        }
    }

    private void UpdateRotation()
    {
        Vector2 mouseDelta = InputManager.Input.Player.Move.ReadValue<Vector2>();

        _force = _forceVertical.CalcRotation(_force, mouseDelta.y);

        _currentRotation.y = _horizontal.CalcRotation(_currentRotation.y, mouseDelta.x);
        transform.localRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);        
    }


    #region Input events for start / end click
    //Start hold
    private void Fire_performed(InputAction.CallbackContext obj)
    {
        if (!_isHeld && _onCoolDown)
        {
            return;
        }

        _isHeld = _isToggle ? !_isHeld : true;
        
        if (_isHeld)
        {
            StartThrow();
        }
        else
        {
            CompleteThrow();
        }
    }

    //Released
    private void Fire_completed(InputAction.CallbackContext obj)
    {
        if (!_isHeld || _isToggle)
        {
            return;
        }

        CompleteThrow();
    }

    //Canceled Fire
    private void Cancel_performed(InputAction.CallbackContext obj)
    {
        _isHeld = false;

        Destroy(_currentBall);
        ToggleIndicator(false);
    }

    private void StartThrow()
    {
        ToggleIndicator(true);
        UpdateLineColor(_PrefabPicker.GetColor());
        ResetSelf();

        SpawnNextThrownObject();

        OnStartThrow.Invoke();
    }

    private void CompleteThrow()
    {
        ToggleIndicator(false);
        _isHeld = false;

        ThrowItem(_currentBall);
        AkSoundEngine.PostEvent("Play_ThrowYarn", gameObject);
        _currentBall = null;

        _PrefabPicker.Remove();
        OnNextColorChange.Invoke(GetNextColors());

        StartCoroutine(RunCoolDown(_afterFireCD));
        OnThrow.Invoke();
    }

    #endregion

    private void ResetSelf()
    {
        transform.rotation = Camera.main.transform.rotation;
        _currentRotation = _rotationOffset;

        _force = (_forceVertical.Min + _forceVertical.Max) * _startForceMulti;
    }

    #region Spawn Thrown Objects
    private void SpawnNextThrownObject()
    {
        GameObject prefab = _PrefabPicker.GetPrefab();

        Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
        _mass = rigidbody.mass;
        _drag = rigidbody.drag;
        _radius = prefab.transform.localScale.x * prefab.GetComponent<SphereCollider>().radius;

        _currentBall = Instantiate(prefab, StartOffset, Quaternion.identity, transform);
        _currentBall.GetComponent<SphereCollider>().enabled = false; // No physics collision until toss
    }
    #endregion

    #region Throw Item
    private void ThrowItem(GameObject go)
    {
        EnableThrownObject(go);
        Rigidbody rb = go.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(_forceVector, ForceMode.Impulse);
    }

    private void EnableThrownObject(GameObject go)
    {
        go.transform.SetParent(null, true);
        go.GetComponent<SphereCollider>().enabled = true; // Reactivate physics collision
    }
    private void ToggleIndicator(bool state)
    {
        _lineRenderer.enabled = state;
        _indicator.SetActive(state);
    }

    #endregion

    #region Line Render
    private void UpdateLineColor(Color color)
    {
        _lineRenderer.material.SetColor("_Color", color);
    }

    /// <summary>
    /// Draws by calc trajectory w/drag & stops on 1st collision or time travelled
    /// </summary>
    /// <param name="ForceVector"></param>
    private bool DrawWithDrag(Vector3 ForceVector)
    {
        _lineRenderer.positionCount = _linePoints;
        float time = _totalTime / _linePoints;

        Vector3 currentPos = StartOffset;
        Vector3 currentVelocity = ForceVector / _mass;

        for (int i = 0; i < _linePoints; i++)
        {
            _lineRenderer.SetPosition(i, currentPos);

            currentVelocity += Physics.gravity * time;
            currentVelocity *= (1 - _drag * time);

            //Sphere Cast to get accurate hit. Ray didn't account for radius.
            Vector3 delta = currentVelocity * time;
            if (Physics.SphereCast(currentPos, _radius, delta.normalized, out RaycastHit hit, delta.magnitude, _layerMask))
            {
                //Get accurate center hit position. 'hit.point' snaped center to point.
                _lineRenderer.SetPosition(i, currentPos + hit.distance * delta.normalized);
                _lineRenderer.positionCount = i + 1;
                return true;
            }

            currentPos += delta;
        }

        return false;
    }

    #endregion
    /// <summary>
    /// Helper function to calc Vector3 directional offsets
    /// </summary>
    /// <param name="transform"></param><param name="offset"></param>
    /// <returns></returns>
    private Vector3 CalcOffset(Transform transform, Vector3 offset)
    {
        return transform.position +
            offset.x * transform.forward +
            offset.y * transform.up +
            offset.z * transform.right;
    }

    IEnumerator RunCoolDown(float time)
    {
        _onCoolDown = true;
        yield return new WaitForSeconds(time);
        _onCoolDown = false;
    }

    public Queue<Color> GetNextColors()
    {
        return _PrefabPicker.NextColors;
    }
}
