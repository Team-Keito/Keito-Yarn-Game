using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSphereGizmo : MonoBehaviour
{
    [SerializeField] private Color _color = Color.blue;
    [SerializeField] private float _dim = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _dim);
    }
}
