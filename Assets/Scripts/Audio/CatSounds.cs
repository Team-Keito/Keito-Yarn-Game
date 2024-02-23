using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSounds : MonoBehaviour
{


    public void OnScoredEvent(float score, bool isFavoriteColor)
    {
        if (isFavoriteColor)
        {
            OnFavoriteColor();
        }
        else if (score > 1)
        {
            OnHighScore();
        }
        else
        {
            DefaultScoreSound();
        }
    }

    public void OnFavoriteColor()
    {
        Debug.Log("Favorite Color");
    }

    public void OnHighScore()
    {
        Debug.Log("High Score");
    }

    public void DefaultScoreSound()
    {
        Debug.Log("Default Score Sound");
    }
}
