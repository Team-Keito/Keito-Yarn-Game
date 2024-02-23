using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatYarnInteraction : MonoBehaviour
{
    public UnityEvent<float, bool> OnCatScored;

    [SerializeField, Tooltip("Tag for Yarnball")]
    private TagSO _yarnTag;

    public GameObject FavoriteColor;




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
            if(FavoriteColor)

            //TODO: Add in color check / maybe pass more ball details.

            OnCatScored.Invoke(collision.transform.localScale.x, false);
            Destroy(collision.gameObject);
        }
    }
}
