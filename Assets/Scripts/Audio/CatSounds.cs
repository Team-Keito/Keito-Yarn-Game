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
        else if (score > 15)
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

    }

    public void OnHighScore()
    {

    }

    public void DefaultScoreSound()
    {

    }
}
