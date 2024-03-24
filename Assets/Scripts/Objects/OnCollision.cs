using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ComponentHelper
{
    public class OnCollision : MonoBehaviour
    {
        public TagSO _tag;
        public UnityEvent OnCollisionEvent;

        private void OnCollisionEnter(Collision collision)
        {
            if (_tag.Compare(collision.gameObject))
            {
                OnCollisionEvent.Invoke();
            }
        }
    }
}
