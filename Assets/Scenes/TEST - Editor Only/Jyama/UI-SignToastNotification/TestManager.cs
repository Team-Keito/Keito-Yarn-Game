using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



namespace TestSpace
{
    public class TestManager : MonoBehaviour
    {
        [SerializeField] private SignManager _signObj;

        private SignPostTypes[] list = (SignPostTypes[])System.Enum.GetValues(typeof(SignPostTypes));
        private int index = 0;

        private void Start()
        {
            _signObj = FindFirstObjectByType<SignManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _signObj.Open((SignPostTypes)list.GetValue(index++));

                index %= list.Length;
            }
        }
    }
}

