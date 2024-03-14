using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Collectable))]


public class AudioCollectable : MonoBehaviour
{
    private string collectSound = "Play_Collect";
    private string collectableNearSound = "Play_Collectable_Near";
    private void OnCollectEvent(CollectableSO data)
    {
        Debug.Log($"{data.name} Collected Called");
        AkSoundEngine.PostEvent(collectSound, gameObject);
    }

    private void OnNearHit(CollectableSO data)
    {
        Debug.Log($"{data.name} Near Hit Called");
        AkSoundEngine.PostEvent(collectableNearSound, gameObject);
    }


    #region Setup
    private Collectable _collectable;
    void Awake()
    {
        _collectable = GetComponent<Collectable>();
    }

    private void OnEnable()
    {
        _collectable.OnCollect.AddListener(OnCollectEvent);
        _collectable.OnNearHit.AddListener(OnNearHit);
    }

    private void OnDisable()
    {
        _collectable.OnCollect.RemoveListener(OnCollectEvent);
        _collectable.OnNearHit.RemoveListener(OnNearHit);
    }

    #endregion
}
