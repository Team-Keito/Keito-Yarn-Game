using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextColorUI : MonoBehaviour
{
    [SerializeField] SlingShot _slingshot; //probably convert to parent class?

    [SerializeField] RawImage[] _Colors;

    public void UpdateNextColor(Queue<Color> Colors)
    {
        int count = 0;
        foreach(Color color in Colors)
        {
            _Colors[count++].color = color;
        }
    }
}
