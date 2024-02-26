using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] private ColorSO _color;

    public ColorSO Color => _color;
}
