using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatYarnInteraction : MonoBehaviour
{
    public UnityEvent<float, bool> OnCatScored;

    [SerializeField, Tooltip("Tag for Yarnball")]
    private TagSO _yarnTag;

    public ColorSO FavoriteColor;

    /// <summary>
    /// Trigger event when yarn ball collides. Uses Tag to check.
    /// 
    /// if want to use layerMask: ((_layerMask & (1 << collision.gameObject.layer))  != 0)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_yarnTag.Tag))
        {
            bool isFavorite = FavoriteColor == collision.gameObject.GetComponent<ColorController>().Color;

            OnCatScored.Invoke(collision.transform.localScale.x, isFavorite);
            Destroy(collision.gameObject);
        }
    }
}
