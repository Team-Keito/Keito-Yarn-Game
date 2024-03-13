using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectTriggerSystem : MonoBehaviour
{
    public UnityEvent OnYarnBallHit = new();

    private int hitCounter = 0;

    [SerializeField, Tooltip("Maximum number of hits before the \"OnYarnBallHit\" event stops being called. " +
        "Leave at zero to call it every time the yarn ball enters.")]
    private int maxHitCounter = 0;

    private Rigidbody objectRb;

    [SerializeField, Tooltip("Tag for yarnball")]
    private TagSO yarnTagSO;

    // Start is called before the first frame update
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();

        OnYarnBallHit.AddListener(TestCollision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (objectRb && collision.gameObject.CompareTag(yarnTagSO.Tag))
        {
            if (maxHitCounter == 0)
                OnYarnBallHit.Invoke();
            else if (hitCounter < 1 || hitCounter < maxHitCounter)
            {
                OnYarnBallHit.Invoke();
                hitCounter++;
            }
        }
    }

    private void TestCollision()
    {
        if(maxHitCounter == 0)
            Debug.Log("Hit repeateatly");
        else if(hitCounter < 1|| hitCounter < maxHitCounter)
        {
            Debug.Log("Times hit: " + (hitCounter + 1) + ". Times left before this message stops printing: " + ((maxHitCounter - hitCounter) - 1));
        }
    }

    private void OnDestroy()
    {
        OnYarnBallHit.RemoveListener(TestCollision);
    }

    private void OnDisable()
    {
        OnYarnBallHit.RemoveListener(TestCollision);
    }
}
