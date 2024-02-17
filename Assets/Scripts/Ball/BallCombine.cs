using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallCombine : MonoBehaviour
{
    public UnityEvent OnCombine;

    [SerializeField] private Color _color;

    [Space(5)]
    [SerializeField] private float _massMultiplier = 1f;
    [SerializeField] private float _massCap = 5f;

    [Space(5)]
    [SerializeField] private float _scaleMultiplier = 1f;
    [SerializeField] private float _scaleCap = 5f;

    private Rigidbody _rigidBody;
    private Vector3 _scaleVectorCap;

    public Color Color => _color;

    void Start()
    {
         SetColor(_color);
        _rigidBody = GetComponent<Rigidbody>();

        _scaleVectorCap = new Vector3(_scaleCap, _scaleCap, _scaleCap);
    }

    public void SetColor(Color color)
    {
        _color = color;
        GetComponent<Renderer>().material.color = _color;
    }

    /// <summary>
    /// Combine balls scale and mass.
    /// Prevent combine if total greater than either cap.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BallCombine>(out BallCombine hitBall) && hitBall.Color == Color)
        {
            Vector3 combinedScale = transform.localScale + hitBall.transform.localScale * _scaleMultiplier;
            float combinedMass = _rigidBody.mass + collision.rigidbody.mass * _massMultiplier;

            if (combinedScale.x > _scaleVectorCap.x || combinedMass > _massCap)
            {
                return;            
            }

            if (transform.position.y > hitBall.transform.position.y)
            {
                Destroy(collision.gameObject);

                //Combine / absorb the mass
                transform.localScale = Vector3.Min(combinedScale, _scaleVectorCap);
                _rigidBody.mass = Mathf.Min(combinedMass, _massCap);

                OnCombine.Invoke();
            }
        }
    }
}