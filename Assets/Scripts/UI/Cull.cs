using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cull : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Debug.Log("culled");
        Destroy(gameObject);
    }
}
