using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreProgressController : MonoBehaviour
{
    [SerializeField] Sprite[] _sprites;
    [SerializeField] Image _progressBar;
    [SerializeField] Image _catFace;

    [System.NonSerialized]
    public int Total, Score;
    

    public void Add(int score)
    {
        SetScore(Score + score);
    }

    public void SetScore(int score)
    {
        Score = score;
        float amount = (float)score / Total;

        _progressBar.fillAmount = amount;

        int index = Mathf.Max((int)(amount / (1f / _sprites.Length)), _sprites.Length-1);

        _catFace.sprite = _sprites[index];
    }

}
