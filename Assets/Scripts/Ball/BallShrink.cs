using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallShrink : MonoBehaviour
{
    public UnityEvent OnSmallestSize;

    [SerializeField] private LayerMask _groundLayer;

    [SerializeField, Tooltip("Minimum ball scale")] 
    private float _minSize = 0.2f;

    [SerializeField, Tooltip("Multiplier of shrink rate")] 
    private float _shrinkMultiplier = 0.5f;

    [SerializeField, Tooltip("Max shrink rate")] 
    private float _shrinkMaxRate = 0.1f;

    [SerializeField, Tooltip("Threshold speed to start shrink, based on horizontal magnitude")] 
    private float _shrinkThresholdSpeed = 0.1f;

    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;
    
    private float _radius;
    private Vector3 _minVectorSize;
    private bool _isSmallest = false;

    private float _scaledRadius => _radius * transform.localScale.x + 0.1f;
    private bool _isGrounded => Physics.Raycast(transform.position, Vector3.down, _scaledRadius, _groundLayer);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();

        _trailRenderer.emitting = false;
        _trailRenderer.startColor = GetComponent<Renderer>().material.color;

        _radius = GetComponent<SphereCollider>().radius;

        _minVectorSize = new Vector3(_minSize, _minSize, _minSize);
    }

    private void FixedUpdate()
    {
        Vector3 horizantalVelocity = _rigidbody.velocity;
        horizantalVelocity.y = 0;

        if (horizantalVelocity != Vector3.zero && horizantalVelocity.magnitude > _shrinkThresholdSpeed && _isGrounded)
        {
            _trailRenderer.emitting = true;
            ShrinkBall(horizantalVelocity.magnitude);
             
            if(!_isSmallest && transform.localScale == _minVectorSize)
            {
                HandleSmallestSize();
            }
        }
        else
        {
            _trailRenderer.emitting = false;
        }
    }

    /// <summary>
    /// Shrinks ball based on movement speed. 
    /// </summary>
    /// <param name="magnitude"></param>
    private void ShrinkBall(float magnitude)
    {
        float rate = Mathf.Min(Mathf.Abs(magnitude) * _shrinkMultiplier, _shrinkMaxRate);

        transform.localScale -= new Vector3(rate, rate, rate) * Time.fixedDeltaTime;
        transform.localScale = Vector3.Max(transform.localScale, _minVectorSize);

        _rigidbody.mass -= rate * Time.fixedDeltaTime;
        _rigidbody.mass = Mathf.Max(_rigidbody.mass, _minSize);
    }

    /// <summary>
    /// Called on combined to re-enabled
    /// </summary>
    public void Reset()
    {
        gameObject.SetActive(true);
        _isSmallest = false;
    }

    private void HandleSmallestSize()
    {
        OnSmallestSize.Invoke();
        _isSmallest = true;
        gameObject.SetActive(false);
    }
}
