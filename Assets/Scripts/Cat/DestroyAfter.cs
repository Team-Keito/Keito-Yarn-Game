using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float _time = 5f;

    void Start()
    {
        Destroy(gameObject, _time);
    }
}
