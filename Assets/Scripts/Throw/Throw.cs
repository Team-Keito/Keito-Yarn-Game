using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Throw : Base_InputSystem
{
    [SerializeField] private GameObject _indicator;
    [SerializeField] private GameObject[] _throwPrefabs;

    [Space(5)]
    [SerializeField] private Vector3 _postionOffset;
    [SerializeField, Tooltip("x & y are flipped cause unity euler")] 
    private Vector2 _rotationOffset;

    [SerializeField] private float _coolDownLength = 0.7f;

    [Space(5)]
    [SerializeField] private int _linePoints = 25;
    [SerializeField] private float _totalTime = 2f;
    [SerializeField] private LayerMask _layerMask;

    [Space(5)]
    [SerializeField] private float _clampVertical = 85f;
    [SerializeField] private float _clampHorizontal = 85f;
    [SerializeField] private float _rotationSpeed = 5f;

    [Space(5)]
    [SerializeField] private bool _useRandomForce = true;
    [SerializeField] private float _minForce = 10f;
    [SerializeField] private float _maxForce = 25f;
    [SerializeField] private float _forceChangeSpeed = 0.8f;

    public UnityEvent<float> OnPowerChange;
    public UnityEvent OnThrow;
    public UnityEvent OnStartThrow;

    private LineRenderer _lineRenderer;

    private float mass, drag;
    private GameObject current;

    private bool isHeld = false;
    private Vector2 _currentRotation;

    private float timeRandomForce = 0f;

    private float _force = 20f;
    private Vector3 _forceVector;
    private bool _onCoolDown;
    

    private Vector3 startOffset => CalcOffset(Camera.main.transform, _postionOffset);

    

    private void Start()
    {
        _force = (_minForce + _maxForce) / 2;

        _lineRenderer = gameObject.GetComponent<LineRenderer>();

        _input.Player.Fire.started += Fire_started;
        _input.Player.Fire.canceled += Fire_canceled;

        _input.Player.Cancel.performed += Cancel_performed;

        transform.position = CalcOffset(Camera.main.transform, _postionOffset);
        transform.SetParent(Camera.main.transform, true);
    }

    private void Cancel_performed(InputAction.CallbackContext obj)
    {
        isHeld = false;

        Destroy(current);
        DisableThrower();
    }

    private void Update()
    {
        if (isHeld)
        {
            UpdateForce();
            UpdateRotation();

            _forceVector = _force * transform.forward;

            DrawWithDrag(_forceVector);
            _indicator.transform.position = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);


            current.transform.position = startOffset;
        }      
    }

    private void UpdateForce()
    {
        if (_useRandomForce)
        {
            timeRandomForce += Time.deltaTime * _forceChangeSpeed;
            float delta = Mathf.PingPong(timeRandomForce, 1);

            OnPowerChange.Invoke(delta);
            _force = Mathf.Lerp(_minForce, _maxForce, delta);
        }        
    }

    private void UpdateRotation()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        _currentRotation.y += mouseDelta.x * Time.deltaTime * _rotationSpeed;
        _currentRotation.x += -mouseDelta.y * Time.deltaTime * _rotationSpeed;

        _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_clampVertical, _clampVertical);
        _currentRotation.y = Mathf.Clamp(_currentRotation.y, -_clampHorizontal, _clampHorizontal);

        transform.localRotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
    }


    #region Input events for start / end click
    private void Fire_started(InputAction.CallbackContext obj)
    {
        if (_onCoolDown)
        {
            return;
        }

        isHeld = true;
        timeRandomForce = 0.5f; //start Random force at average

        _lineRenderer.enabled = true;
        _indicator.SetActive(true);

        transform.rotation = Camera.main.transform.rotation;
        _currentRotation = _rotationOffset;

        SpawnNextThrownObject();
        UpdateLineColor(current);

        OnStartThrow.Invoke();

        StartCoroutine(RunCoolDown());
    }

    private void Fire_canceled(InputAction.CallbackContext obj)
    {
        if (!isHeld)
        {
            return;
        }

        isHeld = false;

        ThrowItem(current);
        current = null;
        DisableThrower();

        OnThrow.Invoke();
    }
    #endregion

    private void DisableThrower()
    {
        _lineRenderer.enabled = false;
        _indicator.SetActive(false);
    }


    private void UpdateLineColor(GameObject obj)
    {
        Color color = obj.GetComponent<Renderer>().material.color;
        _lineRenderer.material.SetColor("_Color", color);
    }


    #region Spawn Thrown Objects
    private void SpawnNextThrownObject()
    {
        GameObject prefab = GetNextPrefab();

        Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
        mass = rigidbody.mass;
        drag = rigidbody.drag;

        current = Instantiate(prefab, startOffset, Quaternion.identity, transform);
    }

    private GameObject GetNextPrefab()
    {
        //TODO: Replace code here for pseudo random spawner if needed.

        int RandInt = Random.Range(0, _throwPrefabs.Length);
        return _throwPrefabs[RandInt];
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
    }
    #endregion

    /// <summary>
    /// Draws by calc trajectory w/drag & stops on 1st collision or time travelled
    /// </summary>
    /// <param name="ForceVector"></param>
    private bool DrawWithDrag(Vector3 ForceVector)
    {
        _lineRenderer.positionCount = _linePoints;
        float time = _totalTime / _linePoints;

        Vector3 currentPos = startOffset;
        Vector3 currentVelocity = ForceVector / mass;

        for(int i = 0; i < _linePoints; i++)
        {
            _lineRenderer.SetPosition(i, currentPos);

            currentVelocity += Physics.gravity * time;
            currentVelocity *= (1 - drag * time);
            
            Vector3 delta = currentVelocity * time;
            if (Physics.Raycast(currentPos, delta.normalized, out RaycastHit hit, delta.magnitude, _layerMask))
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return true;
            }

            currentPos += delta;
        }

        return false;
    }

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


    IEnumerator RunCoolDown()
    {
        _onCoolDown = true;
        yield return new WaitForSeconds(_coolDownLength);
        _onCoolDown = false;
    }
}
