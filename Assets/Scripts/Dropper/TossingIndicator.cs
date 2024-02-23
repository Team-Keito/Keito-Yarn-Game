using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TossingIndicator : MonoBehaviour
{
    [SerializeField] ThrowDropper _thrower;
    [SerializeField] GameObject _ballSpawnIndicator;
    [SerializeField] GameObject _ballHitIndicator;
    [SerializeField] LineRenderer _lineIndicator;
    [SerializeField, Range(3, 1000)] int _linePoints = 25;
    [SerializeField] float _totalTime = 2f;


    private Material _spawnIndicatorMaterial;
    // Start is called before the first frame update
    void Start()
    {
        if (!_thrower) _thrower = GetComponent<ThrowDropper>();
        _spawnIndicatorMaterial = _ballSpawnIndicator.GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary for testing
        if (_thrower.IsHolding)
        {
            // Show trajectory
            LineIndicatorTrajectory();
        }
        else
        {
            // Show drop line
            LineIndicatorDrop();
        }
    }

    private void LineIndicatorDrop()
    {
        _spawnIndicatorMaterial.color = _thrower.YarnColor;
        _lineIndicator.material.SetColor("_Color", _thrower.YarnColor);
        _lineIndicator.positionCount = _linePoints;
        float time = _totalTime / _linePoints;

        // First placed at top
        _lineIndicator.SetPosition(0, _thrower.transform.position);
        // Rest place where hit
        var dropPosition = _thrower.CalculatePosition();
        for (int i = 1; i < _linePoints; i++)
        {
            _lineIndicator.SetPosition(i, dropPosition);
        }
        _ballHitIndicator.SetActive(false);
    }

    private void LineIndicatorTrajectory()
    {
        // Update indicators from _thower data
        _ballHitIndicator.SetActive(true);
        _spawnIndicatorMaterial.color = _thrower.YarnColor;
        _lineIndicator.material.SetColor("_Color", _thrower.YarnColor);
        DrawWithDrag(_thrower.CalculateTossForce(), _thrower.YarnRadius, _thrower.YarnMass, _thrower.YarnDrag);
    }

    /// <summary>
    /// Draws by calc trajectory w/drag & stops on 1st collision or time travelled
    /// </summary>
    /// <param name="ForceVector"></param>
    private bool DrawWithDrag(Vector3 ForceVector, float radius, float mass, float drag)
    {
        _lineIndicator.positionCount = _linePoints;
        float time = _totalTime / _linePoints;

        Vector3 currentPos = _thrower.YarnSpawnPosition;
        Vector3 currentVelocity = ForceVector / mass;

        for (int i = 0; i < _linePoints; i++)
        {
            _lineIndicator.SetPosition(i, currentPos);

            currentVelocity += Physics.gravity * time;
            currentVelocity *= (1 - drag * time);

            Vector3 delta = currentVelocity * time;
            if (Physics.SphereCast(currentPos, radius, delta.normalized, out RaycastHit sphereHit, delta.magnitude, _thrower.ThrowerMask))
            {
                _ballHitIndicator.transform.position = sphereHit.point;
            }
            if (Physics.SphereCast(currentPos, 0.001f, delta.normalized, out RaycastHit hit, delta.magnitude, _thrower.ThrowerMask))
            {
                _lineIndicator.SetPosition(i, hit.point);
                _lineIndicator.positionCount = i + 1;
                return true;
            }

            currentPos += delta;
        }

        return false;
    }
}
