using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Color _color;

    public Color Color { get => _color; }

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Renderer>().material.color = _color;
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.TryGetComponent<Ball>(out Ball hitBall) && hitBall.Color == _color)
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
