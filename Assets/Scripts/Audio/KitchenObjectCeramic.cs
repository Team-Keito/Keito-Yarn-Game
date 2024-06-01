using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectCeramic : MonoBehaviour
{
    public AK.Wwise.Event ceramicCollisionSound; private bool isInitialized = false;

    private void Start()
    {

        Invoke("SetInitialized", 2f); // the delay 
    }

    private void SetInitialized()
    {
        isInitialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isInitialized)
            return;
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        ceramicCollisionSound.Post(gameObject);

    }
}
