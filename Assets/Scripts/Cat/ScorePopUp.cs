using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Manager.Score;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textPrefab;
    [SerializeField] private  Vector3 _offset = new Vector3(0, 2, 0);

    [SerializeField] private Color _default = Color.white;
    [SerializeField] private Color _bonusColor = Color.yellow;
    [SerializeField] private bool _useFavoriteColor = false;

    private Animator _animator;
    private void Start()
    {
        _animator = _textPrefab.gameObject.GetComponent<Animator>();
    }

    public void OnScoredEvent(ScoreData data)
    {
        TextMeshPro text = Instantiate(_textPrefab, transform.position + _offset, Quaternion.identity);

        if (data.bonus > 0)
        {
            _animator.Play("HighScore");
            BonusScore(text, data);
        }
        else
        {
            _animator.Play("BaseScore");
            BaseScore(text, data);
        }
    }

    private void BaseScore(TextMeshPro text, ScoreData data)
    {
        text.text = $"+{data.value}";
        text.color = _default;
    }

    private void BonusScore(TextMeshPro text, ScoreData data)
    {
        text.fontStyle = FontStyles.Italic;
        text.fontWeight = FontWeight.Bold;

        string multi = data.multiplier != 1 ? $" ({data.multiplier}x)" : ""; 

        text.text = $"+{data.value}{multi}";

        text.color = _useFavoriteColor ? data.color.Color : _bonusColor;
    }
}
