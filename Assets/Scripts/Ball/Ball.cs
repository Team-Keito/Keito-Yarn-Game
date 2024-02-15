using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private YarnColor _color;
    public Color Color => _color.Color;

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        if (_color)
        {
            GetComponent<Renderer>().material.color = Color;
        }        
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void SetColor(YarnColor color)
    {
        _color = color;
        GetComponent<Renderer>().material.color = Color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball hitBall) && hitBall.Color == Color)
        {
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
