using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    [SerializeField] private int _linePoints = 25;
    [SerializeField] private float _totalTime = 2f;
    [SerializeField] private LayerMask _layerMask;

    private LineRenderer _lineRenderer;

    private float mass, drag, _timeBetween;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _linePoints;
        _timeBetween = _totalTime / _linePoints;
    }

/*    private void OnValidate()
    {
        _lineRenderer.positionCount = _linePoints;
        _timeBetween = _totalTime / _linePoints;
    }*/

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 CalcVelocity(float force, float mass)
    {
        return CalcVelocity(Camera.main.transform.forward, force, mass);
    }

    public Vector3 CalcVelocity(Vector3 direction, float force, float mass)
    {
        return force * direction / mass;
    }


    private void DrawWithDrag(Vector3 currentPos, Vector3 currentVelocity, float mass, float drag)
    {
        float time = _totalTime / _linePoints;
        _lineRenderer.positionCount = _linePoints;

        for (int i = 0; i < _linePoints; i++)
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
