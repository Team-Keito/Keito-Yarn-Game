using Manager.Score;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/*
* When yarn ball “collides“ with the collectable, the collectable disappears and an event for collecting fires off.
* We may attach game manager to add to the score as a bonus for the player
* When a yarn ball is near by and the player has not noticed it before, we fire an event for yarn nearby.
* When the collectable is collect, it will no longer spawn in the level.
*/

[RequireComponent(typeof(AudioCollectable))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableSO _collectableData;
    [SerializeField] private bool _hideCollected = false;
    [SerializeField] private float _nearHitRadius = 2f;

    [SerializeField] private TagSO _yarnTag;

    public UnityEvent<CollectableSO> OnCollect;
    public UnityEvent<CollectableSO> OnNearHit;
    private bool _hasPlayedNotification = false;

    private ScorePopUp _scorePopup;

    private void Start()
    {
        if (_hideCollected && _collectableData.isCollected)
        {
            HandleHideCollected();
            return;
        }

        AddSphereTrigger();
        _scorePopup = GetComponent<ScorePopUp>();      
    }

    private void HandleHideCollected()
    {
        Destroy(gameObject);
    }

    private void AddSphereTrigger()
    {
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = _nearHitRadius;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (_yarnTag.Compare(collision.gameObject))
        {
            _collectableData.isCollected = true;
            OnCollect.Invoke(_collectableData);

            _scorePopup?.OnScoredEvent(new ScoreData(0, _collectableData.points));   //TODO fix popup to intergrate easier
            Destroy(gameObject);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!_hasPlayedNotification && _yarnTag.Compare(other.gameObject))
        {
            NextHit next = other.gameObject.AddComponent<NextHit>();
            next.OnNextHit += CheckNextHit;
        }
    }
    
    private void CheckNextHit(GameObject go)
    {
        float distance = Vector3.Distance(go.transform.position, transform.position);

        if (distance < _nearHitRadius && go != gameObject)
        {            
            _hasPlayedNotification = true;
            OnNearHit.Invoke(_collectableData);
        }
    }
}
