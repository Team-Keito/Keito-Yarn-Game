using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawnMeow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_Cat_Meow", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
