using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCubeGizmo : MonoBehaviour
{
    [SerializeField] private Color _color = Color.blue;
    [SerializeField] private Vector3 _dim = new Vector3(1, 0, 1);

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireCube(transform.position, _dim);
    }
}
