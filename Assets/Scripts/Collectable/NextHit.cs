using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextHit : MonoBehaviour
{
    public System.Action<GameObject> OnNextHit;

    private bool _hasHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(_hasHit == false)
        {
            _hasHit = true;
            OnNextHit?.Invoke(gameObject);
            Destroy(this);
        }        
    }
}
