using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatYarnInteraction : MonoBehaviour
{
    public UnityEvent<float, bool> OnCatScored;

    [SerializeField] private ThoughtBubble _thoughtBubble;
    [SerializeField, Tooltip("Tag for Yarnball")]
    private TagSO _yarnTag;

    [SerializeField] private float _minSize = 0.5f;
    
    private ColorSO _favoriteColor;

    public void SetFavoriteColor(ColorSO color)
    {
        _favoriteColor = color;
        _thoughtBubble.ChangeColor(color);
    }

    /// <summary>
    /// Trigger event when yarn ball collides. Uses Tag to check.
    /// 
    /// if want to use layerMask: ((_layerMask & (1 << collision.gameObject.layer))  != 0)
    /// Checks fav
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_yarnTag.Tag))
        {
            if(collision.transform.localScale.x > _minSize)
            {
                AcceptBall(collision);
            }
            else
            {
                RejectBall(collision);
            }
        }
    }

    private void AcceptBall(Collision collision)
    {
        bool isFavorite = _favoriteColor == collision.gameObject.GetComponent<ColorController>().Color;

        OnCatScored.Invoke(collision.transform.localScale.x, isFavorite);
        Destroy(collision.gameObject);
    }

    private void RejectBall(Collision collision)
    {
        _thoughtBubble.RejectBall();
    }
}
