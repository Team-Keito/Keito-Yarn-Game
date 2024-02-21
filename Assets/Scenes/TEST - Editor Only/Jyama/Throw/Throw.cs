using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Throw : MonoBehaviour
{
    [SerializeField] private GameObject _thrownPrefab;
    [SerializeField] private GameObject _indicator;

    [SerializeField] private float _force = 200f;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private int _linePoints = 25;
    [SerializeField] private float _totalTime = 2f;

    [SerializeField] private LayerMask _layerMask;

    private LineRenderer _lineRenderer;

    private float mass, drag;
    private GameObject current;
    private bool isThrown = false;

    private Vector3 startOffset => Camera.main.transform.position +
        _offset.x * Camera.main.transform.forward +
        _offset.y * Camera.main.transform.up +
        _offset.z * Camera.main.transform.right;

    private void Start()
    {
        Rigidbody rigidbody = _thrownPrefab.GetComponent<Rigidbody>();
        mass = rigidbody.mass;
        drag = rigidbody.drag;

        _lineRenderer = gameObject.GetComponent<LineRenderer>();

        current = Instantiate(_thrownPrefab, startOffset, Quaternion.identity, Camera.main.transform);

        DisableThrownObject(current);
        UpdateLineColor(current);
    }


    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isThrown)
            {
                isThrown = false;
                current.GetComponent<Rigidbody>().mass = mass;
            }
            else
            {
                EnableThrownObject(current);

                ThrowItem(current);
                Debug.Log(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1));
            }
        }

        DrawWithDrag();

        _indicator.transform.position = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);

        if (!isThrown)
            current.transform.position = startOffset;
    }

    private void UpdateLineColor(GameObject obj)
    {
        Color color = obj.GetComponent<Renderer>().material.color;
        _lineRenderer.material.SetColor("_Color", color);
    }


    private void EnableThrownObject(GameObject go) 
    {
        Collider collider = go.GetComponent<Collider>();
        collider.enabled = true;

        go.transform.SetParent(null, true);
    }
    private void DisableThrownObject(GameObject go)
    {
        Collider collider = go.GetComponent<Collider>();
        collider.enabled = false;

        go.transform.SetParent(Camera.main.transform, true);
    }

    private void ThrowItem(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce( Camera.main.transform.forward * _force, ForceMode.Impulse);

        isThrown = true;
    }

    private void DrawWithDrag()
    {
        _lineRenderer.positionCount = _linePoints;
        float time = _totalTime / _linePoints;

        Vector3 currentPos = startOffset;
        Vector3 currentVelocity = _force * Camera.main.transform.forward / mass;

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
