using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ComponentHelper
{
    public class OnToggleCollision : MonoBehaviour
    {
        [SerializeField] private TagSO _tag;
        [SerializeField] private bool _state = false;

        public UnityEvent OnToggleOn, OnToggleOff;

        private void OnCollisionEnter(Collision collision)
        {
            Toggle(!_state);
        }

        private void Toggle(bool state)
        {
            if (_state = state)
            {
                OnToggleOn.Invoke();
            }
            else
            {
                OnToggleOff.Invoke();
            }
        }
    }

}
