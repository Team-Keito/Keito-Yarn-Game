using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestNextColorDisplay : MonoBehaviour
{
    [SerializeField] SlingShot _slingshot; //probably convert to parent class?

    [SerializeField] RawImage[] _Colors;

    private void Start()
    {
        

    }

    private void OnEnable()
    {
        _slingshot.OnNextColorChange.AddListener(UpdateNextColor);
    }

    private void OnDisable()
    {
        _slingshot.OnNextColorChange.RemoveListener(UpdateNextColor);
    }

    private void UpdateNextColor(LinkedList<Color> Colors)
    {
        int count = 0;

        foreach(Color color in Colors)
        {
            _Colors[count++].color = color;
        }
    }
}