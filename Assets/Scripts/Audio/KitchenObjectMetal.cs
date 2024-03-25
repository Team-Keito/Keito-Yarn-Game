using UnityEngine;
using AK.Wwise;

public class KitchenObjectMetal : MonoBehaviour
{
    public AK.Wwise.Event metalCollisionSound;
    private bool isInitialized = false;

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
        metalCollisionSound.Post(gameObject);

    }
}