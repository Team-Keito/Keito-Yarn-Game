using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCatEventHandler : MonoBehaviour
{
    private float _score;


    /// <summary>
    /// Plays audio & updates score
    /// Not sure how to play sound with wwise
    /// </summary>
    /// <param name="value"></param>
    public void OnCatScore(float value)
    {
        _score += value;
        Debug.Log(string.Format("Score: {0} Value: {1}", _score, value));
    }
}
