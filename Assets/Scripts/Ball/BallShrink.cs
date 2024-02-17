using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallShrink : MonoBehaviour
{
    public UnityEvent OnSmallestSize;

    [SerializeField] private LayerMask _groundLayer;

    [Space(5)]
    [SerializeField, Range(0.01f, 0.5f), Tooltip("Minimum ball scale")] 
    private float _minSize = 0.2f;

    [SerializeField, Range(0.01f, 1.5f), Tooltip("Multiplier of shrink rate")] 
    private float _shrinkMultiplier = 0.5f;

    [SerializeField, Range(0.01f, 0.5f), Tooltip("Capped shrink rate")] 
    private float _shrinkRateCap = 0.1f;

    [Space(5)]
    [SerializeField, Range(0.1f, 3f), Tooltip("Threshold speed to start shrink, based on horizontal magnitude")] 
    private float _ThresholdSpeed = 0.1f;

    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;
    
    private float _baseRadius;
    private Vector3 _minVectorSize;
    private bool _isSmallest = false;

    private float _scaledRadius => _baseRadius * transform.localScale.y + 0.1f;
    private bool _isGrounded => Physics.Raycast(transform.position, Vector3.down, _scaledRadius, _groundLayer);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();

        _trailRenderer.emitting = false;
        _trailRenderer.startColor = GetComponent<Renderer>().material.color;

        _baseRadius = GetComponent<SphereCollider>().radius;

        _minVectorSize = new Vector3(_minSize, _minSize, _minSize);
    }

    /// <summary>
    /// Checks for ground movement & speed threshold
    /// to shrink ball & toggle trail
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 horizantalVelocity = _rigidbody.velocity;
        horizantalVelocity.y = 0;

        if (_isSmallest && horizantalVelocity == Vector3.zero)
        {
            return;
        }

        if (_isGrounded && horizantalVelocity.magnitude > _ThresholdSpeed)
        {
            _trailRenderer.emitting = true;
            ShrinkBall(horizantalVelocity.magnitude);
             
            if(!_isSmallest && transform.localScale == _minVectorSize)
            {
                TriggerSmallestSize();
            }
        }
        else
        {
            _trailRenderer.emitting = false;
        }
    }

    /// <summary>
    /// Shrinks ball based on movement speed (magnitude). 
    /// </summary>
    /// <param name="magnitude">Speed value</param>
    private void ShrinkBall(float magnitude)
    {
        float rate = Mathf.Min(Mathf.Abs(magnitude) * _shrinkMultiplier, _shrinkRateCap);

        transform.localScale -= new Vector3(rate, rate, rate) * Time.fixedDeltaTime;
        transform.localScale = Vector3.Max(transform.localScale, _minVectorSize);

        _rigidbody.mass -= rate * Time.fixedDeltaTime;
        _rigidbody.mass = Mathf.Max(_rigidbody.mass, _minSize);
    }

    /// <summary>
    /// Called OnCombined to re-enabled
    /// </summary>
    public void Reset()
    {
        _isSmallest = false;
        _trailRenderer.emitting = true;
    }

    private void TriggerSmallestSize()
    {
        OnSmallestSize.Invoke();
        _isSmallest = true;
        _trailRenderer.emitting = false;
    }
}
