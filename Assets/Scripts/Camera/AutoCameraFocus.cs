using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCameraFocus : MonoBehaviour
{
    private CinemachineFreeLook _camera;

    private void Start()
    {
        _camera = FindObjectOfType<CinemachineFreeLook>();

        _camera.Follow = this.transform;
        _camera.LookAt = this.transform;

        Destroy(this);
    }
}
