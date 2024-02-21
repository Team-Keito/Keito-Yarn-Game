using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
   
    public string stateGroupName = "GameStates"; 
    public string stateName = "IngameState"; 
    public string PlayerStateGroupName = "PlayerState"; 
    public string PlayerStateName = "Playing"; 
    public string PlayMusicEvent = "Play_Music";

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetState(stateGroupName, stateName);
        AkSoundEngine.SetState(PlayerStateGroupName, PlayerStateName);
        PostPlayMusicEvent();
    }

    public void PostPlayMusicEvent()
    {
        
        // Check if the event name is valid
        if (!string.IsNullOrEmpty(PlayMusicEvent))
        {
            // Post the Wwise yarn collision event by name
            AkSoundEngine.PostEvent(PlayMusicEvent, gameObject);
        }
        else
        {
            Debug.LogError("Cat-Yarn Collision Sound event name is not specified!");
        }
    }
}
