using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] Material Red, Blue, Green, Reject;
    [SerializeField] private Renderer _render;

    private Camera _camera;    

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    public void ChangeColor(ColorSO color)
    {
        Debug.Log($"{_render} -- {Red}");
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

    public void RejectBall()
    {
        _render.material = Reject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
    }
}
