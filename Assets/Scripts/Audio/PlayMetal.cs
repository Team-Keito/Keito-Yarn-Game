using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayMetal : MonoBehaviour
{
    // Start is called before the first frame update

    public AK.Wwise.Event metalCollisionSound;
    public void PostMetalSound()
    {

        metalCollisionSound.Post(gameObject);

    }
}
