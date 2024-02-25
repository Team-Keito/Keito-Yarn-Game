using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatYarnInteraction : MonoBehaviour
{
    [SerializeField] private ThoughtBubble _thoughtBubble;
    [SerializeField, Tooltip("Tag for Yarnball")]
    private TagSO _yarnTag;

    [SerializeField] private float _minSize = 0.6f;
    [SerializeField, Tooltip("The minimum square velocity at which the cat will reject the yarn ball.\nNOTE: this is velocity^2")]
    private float _minSqrVelocityRejection = 4;

    public UnityEvent<float, bool> OnCatScored;
    public UnityEvent OnRejectBallSize;
    public UnityEvent OnRejectBallForce;
    public UnityEvent<ColorSO> OnFavoriteColor;

    private ColorSO _favoriteColor;

    public void SetFavoriteColor(ColorSO color)
    {
        _favoriteColor = color;
        _thoughtBubble.ChangeColor(color);
        OnFavoriteColor.Invoke(color);
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
            // Find first failure
            if (collision.relativeVelocity.sqrMagnitude >= _minSqrVelocityRejection)
            {
                Debug.Log("Force Reject");
                RejectBallForce(collision);
            }
            else if (collision.transform.localScale.x <= _minSize)
            {
                Debug.Log("Size Reject");
                RejectBallSize(collision);
            }
            else
            {
                AcceptBall(collision);
            }
        }
    }

    private void AcceptBall(Collision collision)
    {
        bool isFavorite = _favoriteColor == collision.gameObject.GetComponent<ColorController>().Color;

        OnCatScored.Invoke(collision.transform.localScale.x, isFavorite);
        Destroy(collision.gameObject);
    }

    private void RejectBallSize(Collision collision)
    {
        _thoughtBubble.RejectBall();
        OnRejectBallSize.Invoke();
    }

    private void RejectBallForce(Collision collision)
    {
        // TODO: Should this be a different thought bubble function? 
        _thoughtBubble.RejectBall();
        OnRejectBallForce.Invoke();
    }
}
