using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeManager : MonoBehaviour
{
    public List<Rigidbody> List;

    public float forceMultiplier = 50;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Yarn");

        foreach(GameObject go in objects)
        {
            List.Add(go.GetComponent<Rigidbody>());
            go.GetComponent<BallShrink>().OnSmallestSize.AddListener(Log);
        }
    }

    private void Log()
    {
        Debug.Log("Shrank");
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List.ForEach((Rigidbody rb) => {
                if (rb)
                {
                    rb.AddForce(Vector3.left * forceMultiplier);
                }                
            });
            Debug.Log("Added Force");
        }
    }
}
