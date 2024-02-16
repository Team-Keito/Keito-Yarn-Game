using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatCollisionHandler : MonoBehaviour
{
    public UnityEvent<int, YarnColorSO> CatScored;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball data = collision.gameObject.GetComponent<Ball>();

            CatScored.Invoke((int)collision.transform.localScale.x, data.YarnColor);
            Destroy(collision.gameObject);
        }        
    }
}
