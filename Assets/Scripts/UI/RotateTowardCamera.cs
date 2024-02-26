using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardCamera: MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Start() 
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
    }
}
