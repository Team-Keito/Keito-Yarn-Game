using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    static public GameManager instance => _instance;

    // Start is called before the first frame update
    void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateScore(int value)
    {
    }
}
