using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FurnatureMovement : MonoBehaviour
{
    public UnityEvent OnFurnatureMove = new();

    private Rigidbody furnatureRb;

    [SerializeField, Tooltip("Tag for yarnball")]
    private TagSO _YarnTag;

    [SerializeField, Tooltip("The velocity limit required for the OnFurnatureMove function to be invoked")]
    private float veloLimit;

    public float VeloLimit
    {
        get { return veloLimit; }
        set { veloLimit = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        furnatureRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (furnatureRb && furnatureRb.velocity.sqrMagnitude > veloLimit)
            OnFurnatureMove.Invoke();
    }
}
