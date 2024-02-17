using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeManager : MonoBehaviour
{
    public GameObject TestObject;

    public float forceMultiplier = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TestObject.GetComponent<Rigidbody>().AddForce(Vector3.left * forceMultiplier);
            Debug.Log("Added Force");
        }
    }
}
