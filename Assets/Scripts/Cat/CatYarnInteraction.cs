using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum RejectType
{
    Color,
    Size,
    Force,
    Damage,
}

public class CatYarnInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Tag for Yarnball")]
    private TagSO _yarnTag;

    [SerializeField] private float _minSize = 0.6f;
    [SerializeField, Tooltip("The minimum square velocity at which the cat will reject the yarn ball.\nNOTE: this is velocity^2")]
    private float _minSqrVelocityRejection = 4;

    [SerializeField] private bool _rejectDamagedBall = true;
    [SerializeField, Tooltip("Reject Non-Favorite Colors")] private bool _rejectOffColor = true;

    public UnityEvent<float, bool> OnCatScored;
    public UnityEvent<RejectType> OnReject;
    public UnityEvent<ColorSO> OnFavoriteColor;

    private ColorSO _favoriteColor;
    public ColorSO FavoriteColor => _favoriteColor;

    public void SetFavoriteColor(ColorSO color)
    {
        _favoriteColor = color;
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
            ColorController colorHit = collision.gameObject.GetComponent<ColorController>();

            if(_rejectOffColor && colorHit.Color != FavoriteColor)
            {
                OnReject.Invoke(RejectType.Color);
                return;
            }

            if (_rejectDamagedBall && colorHit.isDamaged())
            {
                OnReject.Invoke(RejectType.Damage);
                return;
            }

            if (collision.transform.localScale.x <= _minSize)
            {
                if (collision.relativeVelocity.sqrMagnitude >= _minSqrVelocityRejection)
                {
                    OnReject.Invoke(RejectType.Force);
                }
                else
                {
                    OnReject.Invoke(RejectType.Size);
                }                    
            }
            else
            {
                AcceptBall(collision);
            }
        }
    }

    private void AcceptBall(Collision collision)
    {
        ColorController colorHit = collision.gameObject.GetComponent<ColorController>();

        bool isFavorite = _favoriteColor == colorHit.Color && !colorHit.isDamaged();

        OnCatScored.Invoke(collision.transform.localScale.x, isFavorite);
        Destroy(collision.gameObject);
    }
}
