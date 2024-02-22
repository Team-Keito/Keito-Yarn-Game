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
    [SerializeField] float _forceDamping = 10;


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
        // Get where the mouse started holding from
        Vector2 mouseScreenCenter = _thrower.CenterMouse;
        // Get where the mouse currently is
        Vector2 mouseScreenOffset = Mouse.current.position.ReadValue();
        // Find the reflected vector (dampened by some value)
        Vector2 mouseInfo = (mouseScreenOffset - mouseScreenCenter) / _forceDamping;
        // Rotate by the camera's "y" rotation Euler value (depending on the angle)
        mouseInfo = _thrower.SlingshotDirection * Vec2AngleRotate(mouseInfo, -_thrower.CameraYRotation);
        // Update indicators
        _ballHitIndicator.SetActive(true);
        _spawnIndicatorMaterial.color = _thrower.YarnColor;
        _lineIndicator.material.SetColor("_Color", _thrower.YarnColor);
        DrawWithDrag(new Vector3(mouseInfo.x, 0, mouseInfo.y), 1, 1);
        _ballHitIndicator.transform.position = _lineIndicator.GetPosition(_lineIndicator.positionCount - 1);
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
    /// Draws by calc trajectory w/drag & stops on 1st collision or time travelled
    /// </summary>
    /// <param name="ForceVector"></param>
    private bool DrawWithDrag(Vector3 ForceVector, float mass, float drag)
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
            if (Physics.Raycast(currentPos, delta.normalized, out RaycastHit hit, delta.magnitude, _thrower.ThrowerMask))
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
