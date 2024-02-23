using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClickSound : MonoBehaviour
{
    private string buttonClickEventName = "Play_ButtonClick";


    public void PlayButtonClickSound()
    {
        // Post the Wwise event using the specified event name
        AkSoundEngine.PostEvent(buttonClickEventName, gameObject);
    }
}
