using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


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

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            }
        }


        public void Log(string data)
        {
            Debug.Log(data);
        }
    }
}

