using Cinemachine;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField, Tooltip("Default self")] 
    private Transform _target;

    [SerializeField, Tooltip("Default 0.5f")]
    private float _zoomLevel = 0.5f;

    private CinemachineFreeLook _camera;

    private void Start()
    {
        if (!_camera)
        {
            _camera = FindObjectOfType<CinemachineFreeLook>();
        }

        if(!_target)
        {
            _target = this.transform;
        }
    }

    public void Focus()
    {
        _camera.m_YAxis.Value = _zoomLevel;
        ReFocusCamera();
    }

    public void ReFocusCamera()
    {
        _camera.Follow = _target;
        _camera.LookAt = _target;        
    }
}
