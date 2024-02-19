using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public CinemachineFreeLook _camera;

    private void Start()
    {
        if (!_camera) _camera = FindObjectOfType<CinemachineFreeLook>();
    }

    private void OnMouseDown()
    {
        _camera.Follow = this.transform;
        _camera.LookAt = this.transform;
    }
}
