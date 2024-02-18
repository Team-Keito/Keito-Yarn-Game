using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperDebugVisualizer : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image _yarnImage;
    [SerializeField] YarnDropper _dropper;

    void Start()
    {
        if (!_dropper) _dropper = FindObjectOfType<YarnDropper>();
        if (!_yarnImage) _yarnImage = GetComponentInChildren<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _yarnImage.color = _dropper.CurrentColor();
    }
}
