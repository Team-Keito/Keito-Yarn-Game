using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public  CinemachineFreeLook camera;

    private void OnMouseDown()
    {
        camera.Follow = this.transform;
        camera.LookAt = this.transform;
    }
}
