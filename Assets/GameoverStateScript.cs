using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverStateScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetState("GameStates", "Game_over");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
