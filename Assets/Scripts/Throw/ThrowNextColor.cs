using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowNextColor : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image _yarnImage;
    [SerializeField] Throw _throwRef;

    void Start()
    {
        if (!_throwRef) _throwRef = FindObjectOfType<Throw>();
        if (!_yarnImage) _yarnImage = GetComponentInChildren<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _yarnImage.color = _throwRef.GetNextColor;
    }
}
