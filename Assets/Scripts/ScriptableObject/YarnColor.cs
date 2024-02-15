using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class YarnColor : ScriptableObject
{
    [SerializeField] private Color _color;
    public Color Color => _color;
}
