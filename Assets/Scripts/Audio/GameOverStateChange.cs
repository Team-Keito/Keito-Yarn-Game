using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStateChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AkSoundEngine.SetState("GameStates", "Game_over");
        Debug.Log("I change state to Gameover");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
