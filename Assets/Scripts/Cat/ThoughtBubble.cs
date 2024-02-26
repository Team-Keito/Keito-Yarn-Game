using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] private float _rejectShowTime = 1f;
    
    [Space(5)]
    [SerializeField] Material Red, Blue, Green, RejectSmall, RejectFast;
    [SerializeField] private Renderer _render;
       

    private ColorSO _currentColor;
    private Camera _camera;    

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
    }

    public void ChangeColor(ColorSO color)
    {
        _currentColor = color;
        switch (color.name)
        {
            case "Red":
                _render.sharedMaterial = Red;
                break;
            case "Blue":
                _render.sharedMaterial = Blue;
                break;
            case "Green":
                _render.sharedMaterial = Green;
                break;
        }
    }

    public void RejectSmallBall()
    {
        _render.sharedMaterial = RejectSmall;
        StartCoroutine(TempShow());   
    }

    public void RejectFastBall()
    {
        _render.sharedMaterial = RejectFast;
        StartCoroutine(TempShow());
    }

    IEnumerator TempShow()
    {
        yield return new WaitForSeconds(_rejectShowTime);
        ChangeColor(_currentColor);
    }

}
