using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVAudio : MonoBehaviour
{
    public AK.Wwise.Event tvOn;
    public AK.Wwise.Event tvOff;
    public void PlayOn()
    {
        tvOn.Post(gameObject);
    }

    public void PlayOff()
    {
        tvOff.Post(gameObject);
    }
}
