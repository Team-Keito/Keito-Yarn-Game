using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "SO/Color")]
public class ColorSO : ScriptableObject
{
    public string Name;
    public Color Color;
}
