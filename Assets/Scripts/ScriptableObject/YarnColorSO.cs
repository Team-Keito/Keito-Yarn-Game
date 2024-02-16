using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YarnColor", menuName = "Yarn/Color")]
public class YarnColorSO : ScriptableObject
{
    [SerializeField] private Color _color;
    public Color Color => _color;
}
