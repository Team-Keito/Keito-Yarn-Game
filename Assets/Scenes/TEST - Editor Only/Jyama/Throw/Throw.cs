using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Throw : Base_InputSystem
{
    [SerializeField] private GameObject _thrownPrefab;
    [SerializeField] private GameObject _indicator;

    [Space(5)]
    [SerializeField] private float _force = 200f;
    [SerializeField] private Vector3 _offset;

    [Space(5)]
    [SerializeField] private int _linePoints = 25;
    [SerializeField] private float _totalTime = 2f;

    [SerializeField] private LayerMask _layerMask;

    private LineRenderer _lineRenderer;

    private float mass, drag;
    private GameObject current;

    private bool isHeld = false;
    private Vector2 _orginalMousePos;

    private Vector3 startOffset => CalcOffset(Camera.main.transform, _offset);

    private void Start()
    {
        Rigidbody rigidbody = _thrownPrefab.GetComponent<Rigidbody>();
        mass = rigidbody.mass;
        drag = rigidbody.drag;

        _lineRenderer = gameObject.GetComponent<LineRenderer>();

        _input.Player.Fire.started += Fire_started;
        _input.Player.Fire.canceled += Fire_canceled;
    }


    public Vector3 CalcOffset(Transform transform, Vector3 offset)
    {
        return transform.position +
            offset.x * transform.forward +
            offset.y * transform.up +
            offset.z * transform.right;
    }


    private void Fire_started(InputAction.CallbackContext obj)
    {
        _orginalMousePos = Mouse.current.position.ReadValue();
        isHeld = true;

        _lineRenderer.enabled = true;
        _indicator.SetActive(true);

        Destroy(current);
        SpawnNextThrownObject();
        UpdateLineColor(current);
    }

    private void Fire_canceled(InputAction.CallbackContext obj)
    {
        isHeld = false;

        EnableThrownObject(current);
        ThrowItem(current);
        
        _lineRenderer.enabled = false;
        _indicator.SetActive(false);
    }

    private void Update()
    {
/*        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);*/

        if (isHeld)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue() - _orginalMousePos;

            //_force * Camera.main.transform.forward
            
            DrawWithDrag(_force * Camera.main.transform.forward);
            _indicator.transform.position = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
            current.transform.position = startOffset;
        }      
    }

    private void UpdateLineColor(GameObject obj)
    {
        Color color = obj.GetComponent<Renderer>().material.color;
        _lineRenderer.material.SetColor("_Color", color);
    }

    private void EnableThrownObject(GameObject go) 
    {
        go.transform.SetParent(null, true);
    }
    private void SpawnNextThrownObject()
    {
        current = Instantiate(_thrownPrefab, startOffset, Quaternion.identity, Camera.main.transform);
        current.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void ThrowItem(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(Camera.main.transform.forward * _force, ForceMode.Impulse);

        //isThrown = true;
    }

    private void DrawWithDrag(Vector3 ForceVector)
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
                return;
            }

            currentPos += delta;
        }
    }
}
