using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;

    [SerializeField] private bool _showTrailOnDrop = true;

    // Start is called before the first frame update
    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.startColor = GetComponent<Renderer>().material.color;
        _trailRenderer.enabled = _showTrailOnDrop;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _trailRenderer.enabled = true;
    }
}
