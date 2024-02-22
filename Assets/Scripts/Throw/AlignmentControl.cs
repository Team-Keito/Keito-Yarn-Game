using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AlignmentControl
{
    public float Speed = 5f;
    public bool Invert = false;

    public float Min = -45f;
    public float Max = 45f;

    private int _invert => Invert ? -1 : 1;

    public float CalcRotation(float value, float delta)
    {
        return CalcRotation(value, delta, Min, Max);
    }

    public float CalcRotation(float value, float delta, float min, float max)
    {
        value += delta * Time.deltaTime * Speed * _invert;
        return Mathf.Clamp(value, min, max);
    }
}
