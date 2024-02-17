using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatCollisionHandler : MonoBehaviour
{
    public UnityEvent<int> CatScored;

    [SerializeField]
    private LayerMask _ballLayerMask;

    /// <summary>
    /// Pass score based on scale of ball.
    /// Uses layermask or could use CompareTag.
    /// 
    /// TODO: add in check for color / logic to ingore or give less points? 
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.CompareTag("Ball"))
        if ((_ballLayerMask & (1 << collision.gameObject.layer))  != 0)
        {
            CatScored.Invoke((int)collision.transform.localScale.x);
            Destroy(collision.gameObject);
        }        
    }
}
