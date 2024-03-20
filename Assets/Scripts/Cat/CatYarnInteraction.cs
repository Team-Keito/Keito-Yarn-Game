using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatYarnInteraction : MonoBehaviour
{
    [SerializeField, Tooltip("Tag for Yarnball")]
    private TagSO _yarnTag;

    [SerializeField] private float _minSize = 0.6f;
    [SerializeField, Tooltip("The minimum square velocity at which the cat will reject the yarn ball.\nNOTE: this is velocity^2")]
    private float _minSqrVelocityRejection = 4;

    [SerializeField] private bool _rejectDamagedBall = true;

    public UnityEvent<float, bool> OnCatScored;
    public UnityEvent OnRejectBallSize;
    public UnityEvent OnRejectBallForce;
    public UnityEvent OnRejectDamagedBall;
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
            // Find first failure
            if (collision.transform.localScale.x <= _minSize)
            {
                if (collision.relativeVelocity.sqrMagnitude >= _minSqrVelocityRejection)
                {
                    RejectBallForce(collision);
                }
                else
                {
                    RejectBallSize(collision);
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

        if(_rejectDamagedBall && colorHit.isDamaged())
        {
            RejectDamagedBall();
            return;
        }

        bool isFavorite = _favoriteColor == colorHit.Color && !colorHit.isDamaged();

        OnCatScored.Invoke(collision.transform.localScale.x, isFavorite);
        Destroy(collision.gameObject);
    }

    private void RejectDamagedBall()
    {
        //TODO: Add in damaged ball thought bubble?
        OnRejectDamagedBall.Invoke();
    }

    private void RejectBallSize(Collision collision)
    {
        OnRejectBallSize.Invoke();
    }

    private void RejectBallForce(Collision collision)
    {
        // TODO: Should this be a different thought bubble function? 
        OnRejectBallForce.Invoke();
    }
}
