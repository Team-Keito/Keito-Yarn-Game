using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    public UnityEvent OnCollisionEvent;
    public TagSO _tag;

    private void OnCollisionEnter(Collision collision)
    {
        if (_tag.Compare(collision.gameObject))
        {
            OnCollisionEvent.Invoke();
        }        
    }
}
