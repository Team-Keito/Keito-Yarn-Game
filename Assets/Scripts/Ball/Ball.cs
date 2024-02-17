using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public UnityEvent OnCombine;

    [SerializeField] private Color _color;

    [SerializeField] private float _massMultiplier = 1f;
    [SerializeField] private float _massCap = 5f;

    [SerializeField] private float _scaleMultiplier = 1f;
    [SerializeField] private float _scaleCap = 5f;

    private Rigidbody _rigidBody;

    private Vector3 _scaleVectorCap;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball hitBall) && hitBall._color == _color)
        {
            Vector3 combinedScale = transform.localScale + hitBall.transform.localScale * _scaleMultiplier;
            float combinedMass = _rigidBody.mass + collision.rigidbody.mass * _massMultiplier;

            if (combinedScale.x > _scaleVectorCap.x || combinedMass > _massCap)
            {
                return;            
            }

            //Destroy top ball & combine into bottom ball
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