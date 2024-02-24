using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSounds : MonoBehaviour
{
    private string FavoriteYarnColorSound = "Play_FavoriteYarnColor";
    private string DefualtScore = "Play_Cat_Purr";
    private string HighScore = "Play_HighScore";
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
        AkSoundEngine.PostEvent(FavoriteYarnColorSound, gameObject);
    }

    public void OnHighScore()
    {
        Debug.Log("High Score");
        AkSoundEngine.PostEvent(HighScore, gameObject);
    }

    public void DefaultScoreSound()
    {
        Debug.Log("Default Score Sound");
        AkSoundEngine.PostEvent(DefualtScore, gameObject);
    }
}
