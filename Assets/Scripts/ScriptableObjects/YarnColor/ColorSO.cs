using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color", menuName = "SO/Color")]
public class ColorSO : ScriptableObject
{
    public string Name;
    public Color Color;
    // TODO: Remove prefab in favor of material (easier to change and relies on a single prefab)
    public GameObject YarnPrefab;
    public Material YarnMaterial;
}
