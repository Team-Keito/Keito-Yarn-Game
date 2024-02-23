using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FurnatureMovement : MonoBehaviour
{
    public UnityEvent<Vector3> OnFurnatureMove = new();

    private Rigidbody furnatureRb;

    [SerializeField, Tooltip("Tag for yarnball")]
    private TagSO _YarnTag;

    // Start is called before the first frame update
    void Start()
    {
        furnatureRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_YarnTag.Tag))
        {
            OnFurnatureMove.Invoke(furnatureRb.velocity);
        }
    }
}
