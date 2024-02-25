using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class OffScreenIndicator : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Camera _camera;
    [SerializeField] private Image _indicatorUI;
    [SerializeField] private float _imageRotationOffset = 180;

    private Vector3 _center, _boundsCenter;

    #region Setup
    // Start is called before the first frame update
    void Start()
    {
        if (!_camera) _camera = Camera.main;

        Vector3 screen = new Vector3(Screen.width, Screen.height);
        _center = screen / 2;
        _boundsCenter = (screen * 0.9f) / 2;

        if(!_target) enabled = false;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        CheckTarget();
    }
    #endregion
    
    public void UpdateTarget(GameObject go)
    {
        _target = go;
        enabled = true;
    }


    /*    private void OnDrawGizmos()       //Draws line from camera to target
        {
            Vector3 screenpoint = _camera.WorldToScreenPoint(Target.transform.position);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_camera.ScreenToWorldPoint(screenpoint), _camera.transform.position + _camera.transform.forward * 5);
        }*/

    private void CheckTarget()
    {
        Vector3 targetPoint = _camera.WorldToScreenPoint(_target.transform.position);

        bool isOnScreen = targetPoint.z > 0 
            && targetPoint.x >= 0 && targetPoint.x <= Screen.width 
            && targetPoint.y >= 0 && targetPoint.y <= Screen.height;

        if (!isOnScreen)
        {
            _indicatorUI.enabled = true;
            OffscreenTracker(targetPoint);
        }
        else
        {
            _indicatorUI.enabled = false;
        }
    }

    private void OffscreenTracker(Vector3 targetPoint)
    {
        if (targetPoint.z < 0)  
        {
            targetPoint *= -1;  //Fixes backwards tracking
        }

        targetPoint.z = 0;
        targetPoint -= _center;

        float divX = _boundsCenter.x / Mathf.Abs(targetPoint.x);
        float divY = _boundsCenter.y / Mathf.Abs(targetPoint.y);

        float angle;

        if (divX < divY)
        {
            float sign = Mathf.Sign(targetPoint.x);
            angle = Vector3.SignedAngle(Vector3.right, targetPoint, Vector3.forward) ;
            
            targetPoint.x = sign * (_boundsCenter.x);
            targetPoint.y = sign * Mathf.Tan(angle * Mathf.Deg2Rad) * _boundsCenter.x;

            angle -= 90; //rotates to align horizontally
        }
        else
        {
            float sign = Mathf.Sign(targetPoint.y);
            angle = Vector3.SignedAngle(Vector3.up, targetPoint, Vector3.forward);

            targetPoint.y = sign * (_boundsCenter.y);
            targetPoint.x = -sign * Mathf.Tan(angle * Mathf.Deg2Rad) * _boundsCenter.y;
        }

        UpdateElement(targetPoint + _center, angle);
    }

    private void UpdateElement(Vector3 position, float rotation)
    {
        _indicatorUI.gameObject.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, rotation - _imageRotationOffset));
    }
}
