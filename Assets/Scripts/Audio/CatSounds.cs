using Manager.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSounds : MonoBehaviour
{
    private readonly string FavoriteYarnColorSound = "Play_FavoriteYarnColor";
    private readonly string DefualtScore = "Play_Cat_Purr";
    private readonly string HighScore = "Play_HighScore";
    private readonly string RejectBallSizeSound = "Play_CatRefusesYarn";
    private readonly string RejectBallForceSound = "Play_CatRefusesYarn";
    private readonly string RejectDefault = "Play_CatRefusesYarn";

    [SerializeField] private int _highScore = 20;
    public void OnScoredEvent(ScoreData data)
    {
        if (data.value > _highScore)
        {
            OnHighScore();
        }
        else if (data.isFavoriteColor)
        {
            OnFavoriteColor();
        }
        else
        {
            DefaultScoreSound();
        }
    }

    public void HandleRejectType(RejectType type)
    {
        switch (type)
        {
            case RejectType.Force: OnRejectBallForce(); break;
            case RejectType.Size: OnRejectBallForce(); break;

            default: AkSoundEngine.PostEvent(RejectDefault, gameObject); break;
        }
    }

    public void OnFavoriteColor()
    {
        //Debug.Log("Favorite Color");
        AkSoundEngine.PostEvent(FavoriteYarnColorSound, gameObject);
    }

    public void OnHighScore()
    {
        //Debug.Log("High Score");
        AkSoundEngine.PostEvent(HighScore, gameObject);
    }

    public void DefaultScoreSound()
    {
        //Debug.Log("Default Score Sound");
        AkSoundEngine.PostEvent(DefualtScore, gameObject);
    }

    public void OnRejectBallSize()
    {
        // Debug.Log("Reject Size Sound");
        AkSoundEngine.PostEvent(RejectBallSizeSound, gameObject);
    }

    public void OnRejectBallForce()
    {
        AkSoundEngine.PostEvent(RejectBallForceSound, gameObject);
    }
}
