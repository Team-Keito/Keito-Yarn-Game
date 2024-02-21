using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterTag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string yarnTag = "Yarn"; 
    public string soundEventName = "Play_Cat_Purr"; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(yarnTag))
        {
            AkSoundEngine.PostEvent(soundEventName, gameObject);
        }
    }
}
