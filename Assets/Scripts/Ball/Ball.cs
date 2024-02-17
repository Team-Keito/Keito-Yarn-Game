using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private YarnColorSO _yarnColor;

    public YarnColorSO YarnColor => _yarnColor;
    public Color Color => YarnColor.Color;

    private Rigidbody _rigidBody;

    void Awake()
    {
        if (_yarnColor)
        {
            SetColor(YarnColor);
        }        
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void OnValidate()
    {
        if (YarnColor)
        {
            SetColor(YarnColor);
        }
    }

    public void SetColor(YarnColorSO color)
    {
        _yarnColor = color;
        GetComponent<Renderer>().material.color = Color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball hitBall) && hitBall.Color == Color)
        {
            //Destroy top ball & combine into bottom ball
            if (transform.position.y > hitBall.transform.position.y)
            {
                Destroy(gameObject);
            }
            else
            {
                //Combine / absorb the mass
                transform.localScale += hitBall.transform.localScale;
                _rigidBody.mass += collision.rigidbody.mass;
            }
        }
    }
}