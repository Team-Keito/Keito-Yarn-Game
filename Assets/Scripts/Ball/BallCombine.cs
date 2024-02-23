using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallCombine : MonoBehaviour
{
    public UnityEvent OnCombine;
    public UnityEvent OnMaxSize;

    [Space(5)]
    [SerializeField] private float _massMultiplier = 1f;
    [SerializeField] private float _massCap = 5f;

    [Space(5)]
    [SerializeField] private float _scaleMultiplier = 1f;
    [SerializeField] private float _scaleCap = 5f;

    private Rigidbody _rigidBody;
    private Renderer _renderer;
    private Vector3 _scaleVectorCap;

    public string YarnCombineSound = "Play_Yarn_Combine";

    public Color Color => _renderer.material.color;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();

        _scaleVectorCap = new Vector3(_scaleCap, _scaleCap, _scaleCap);
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
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
                OnMaxSize.Invoke();
                return;            
            }

            if (transform.position.y < hitBall.transform.position.y)
            {
                Destroy(collision.gameObject);

                //Combine / absorb the mass
                transform.localScale = Vector3.Min(combinedScale, _scaleVectorCap);
                _rigidBody.mass = Mathf.Min(combinedMass, _massCap);

                OnCombine.Invoke();
                AkSoundEngine.PostEvent(YarnCombineSound, gameObject);
            }
        }
    }
}