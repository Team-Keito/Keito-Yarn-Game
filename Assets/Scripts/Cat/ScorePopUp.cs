using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] TextMeshPro _textPrefab;
    [SerializeField] Vector3 _offset = new Vector3(0, 50, 0);

    [SerializeField] Color _default = Color.white;
    [SerializeField] Color _bonusColor = Color.yellow;

    public void OnScoredEvent(float value, bool isFavoriteColor)
    {
        TextMeshPro text = Instantiate(_textPrefab, transform.position + _offset, Quaternion.identity);
        text.text = $"+{Mathf.RoundToInt(value)}";
        text.color = isFavoriteColor ? _bonusColor : _default;
    }
}
